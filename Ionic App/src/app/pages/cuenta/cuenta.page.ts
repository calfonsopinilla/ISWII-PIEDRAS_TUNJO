import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Usuario } from '../../interfaces/usuario.interface';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-cuenta',
  templateUrl: './cuenta.page.html',
  styleUrls: ['./cuenta.page.scss'],
})
export class CuentaPage implements OnInit {

  editForm: FormGroup;
  updateUser = false;
  usuario: Usuario;
  avatar = undefined;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private userService: UserService,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
    this.crearForm();
  }

  crearForm() {
    this.auth.getUsuario().then(user => {
      this.usuario = user;
      this.editForm = this.fb.group({
        nombre: [user.Nombre, Validators.required],
        apellido: [user.Apellido, Validators.required]
      });
    });
  }

  async onSubmit() {
    const {nombre, apellido} = this.editForm.value;
    this.usuario.Nombre = nombre;
    this.usuario.Apellido = apellido;
    if (this.avatar !== undefined) {
      this.usuario.Icono_url = this.avatar;
    }
    // console.log(this.usuario);
    const updated = await this.auth.actualizarUsuario(this.usuario);
    if (updated) {
      this.updateUser = false;
      this.editForm.setValue({ nombre, apellido });
      this.presentToast('Datos actualizados!');
      this.avatar = undefined;
      this.crearForm();
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'bottom',
      duration: 3000
    });
    await toast.present();
  }

  // getters
  get nombre() {
    return this.editForm.get('nombre');
  }

  get apellido() {
    return this.editForm.get('apellido');
  }

}
