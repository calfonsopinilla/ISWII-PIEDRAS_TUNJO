import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';
import { Storage } from '@ionic/storage';
import { UserService } from '../../services/user.service';
import { UserRegister } from '../../interfaces/user-regster.interface';

@Component({
  selector: 'app-registro',
  templateUrl: './registro.page.html',  
  styleUrls: ['./registro.page.scss'],
})
export class RegistroPage implements OnInit {

  usuario: UserRegister = new UserRegister();

  constructor(
    private userService: UserService,
    private toastCtrl: ToastController,
    private storage: Storage,
    private loadingCtrl: LoadingController
  ) { }

  ngOnInit() {
  }

  // Registrar usuario y enviar Token
  async crearUsuario() {
    this.usuario.id = 0;
    this.usuario.token = null;
    this.usuario.fechaGeneracion = null;
    this.usuario.fechaVencimiento = null; 
    this.usuario.rolId = 2;
    this.usuario.iconoUrl = 'fkodaa';
    this.usuario.aplicacionId = 1;

    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();

    // Servicio para validar numero documento y correo electronico
    if (await !this.validarNumeroDocumentoCorreoElectronico(this.usuario)) {
      // Servicio para crear usuario
      this.userService.crearUsuario(this.usuario)
      .subscribe(
        async (res) => {
          setTimeout(_ => { }, 5000);
          this.presentToast(res['message']);
        },
        (err) => { },
        () => loading.dismiss()
      );
    } else {
      this.presentToast('ERROR: Ya existe el usuario con el mismo número de documento y/o con el mismo correo electrónico');
    }
  }

  private validarNumeroDocumentoCorreoElectronico(user: UserRegister): any {
    this.userService.validarNumeroDocumentoCorreoElectronico(user)
      .subscribe(
        async (res) => {
          setTimeout(_ => { }, 5000);
          if ((res[0] === true)) {
            return true;
          } else {
            return false;
          }
        }
      );
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
      duration: 3000
    });
    await toast.present();
  }
}
