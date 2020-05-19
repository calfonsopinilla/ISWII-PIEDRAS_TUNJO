import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { UserRegister } from '../../../interfaces/user-regster.interface';
import { EmailValidator } from '../../../directives/email.directive';
import { NumeroDocValidator } from '../../../directives/numero-doc.directive';
import { AlertController } from '@ionic/angular';
import { ToastController } from '@ionic/angular';
import { InfoParqueService } from '../../../services/info-parque.service';

@Component({
  selector: 'app-registro',
  templateUrl: './registro.page.html',
  styleUrls: ['./registro.page.scss'],
})
export class RegistroPage implements OnInit {

  formUser: FormGroup;
  userRegister = new UserRegister();
  avatar = undefined;
  terminosCondiciones: string;
  tenYearsBefore: Date;

  constructor(
    private infoParqueService: InfoParqueService,
    private userService: UserService,
    private fb: FormBuilder,
    private emailValidator: EmailValidator,
    private numDocValidator: NumeroDocValidator,
    public alertController: AlertController,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
    this.avatar = "av-1.png";
    this.tenYearsBefore = new Date();
    this.tenYearsBefore.setFullYear(this.tenYearsBefore.getFullYear()-10);    
    this.formUser = this.fb.group({
      correoElectronico: ['', Validators.required,
        [
          this.emailValidator.validateToken.bind(this.emailValidator), // Validar tabla Token Correo
          this.emailValidator.validate.bind(this.emailValidator)
        ]
      ],
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      fechaNacimiento: [undefined, Validators.required],
      clave: ['', [Validators.required, Validators.minLength(5)]],
      tipoDocumento: ['TI', Validators.required],
      numeroDocumento: [
        '',
        [Validators.required, Validators.minLength(6), Validators.maxLength(12)],
        [
          this.numDocValidator.validateToken.bind(this.numDocValidator), // Validar tabla Token Correo
          this.numDocValidator.validate.bind(this.numDocValidator)
        ]
      ]
    });    
    this.infoParqueService.obtenerItemInfo(8)
                            .subscribe(res => {
                              this.terminosCondiciones = res['descripcion'];
                            });
  }

  // Mostrar términos y condiciones
  async mostrarTerminosCondiciones() {

    const checkBox = document.querySelector('ion-checkbox');    
    if (checkBox.getAttribute('aria-checked').toString() === 'false') {

      const alert = await this.alertController.create({
        header: 'Términos y Condiciones',      
        message: this.terminosCondiciones,
        buttons: ['Ok']
      });    
  
      await alert.present();
    }    
  }  

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
      duration: 3000
    });
    await toast.present();
  }

  crearUsuario() {
    // console.log(this.formUser.value);
    this.userRegister = {
      ... this.formUser.value,
      iconoUrl: this.avatar,
      id: 0,
      token: null,
      fechaGeneracion: null,
      fechaVencimiento: null,
      rolId: 2,
      aplicacionId: 1,      
    };    

    this.userRegister.correoElectronico.trim();
    console.log();

    if (this.avatar !== undefined) {
      this.userRegister.iconoUrl = this.avatar;
    }

    const checkBox = document.querySelector('ion-checkbox');    
    if (checkBox.getAttribute('aria-checked').toString() === 'true') {
      this.userService.crearUsuario(this.userRegister);
    } else {
      this.presentToast('ERROR: Debe aceptar los términos y condiciones');
    }   
  }

  keypressNumDoc(e: any) {
    const key = Number(e.key);
    if (isNaN(key)) {
      e.preventDefault();
    }
  }

  get correo() {
    return this.formUser.get('correoElectronico');
  }

  get nombre() {
    return this.formUser.get('nombre');
  }

  get apellido() {
    return this.formUser.get('apellido');
  }

  get clave() {
    return this.formUser.get('clave');
  }

  get numeroDoc() {
    return this.formUser.get('numeroDocumento');
  }

  get fechaNac() {
    return this.formUser.get('fechaNacimiento');
  }

}
