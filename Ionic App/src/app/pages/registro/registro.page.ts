import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserRegister } from '../../interfaces/user-regster.interface';
import { EmailValidator } from '../../directives/email.directive';
import { NumeroDocValidator } from '../../directives/numero-doc.directive';

@Component({
  selector: 'app-registro',
  templateUrl: './registro.page.html',
  styleUrls: ['./registro.page.scss'],
})
export class RegistroPage implements OnInit {

  formUser: FormGroup;
  userRegister = new UserRegister();
  avatar = 'av-1.png';

  constructor(
    private userService: UserService,
    private fb: FormBuilder,
    private emailValidator: EmailValidator,
    private numDocValidator: NumeroDocValidator
  ) { }

  ngOnInit() {
    this.formUser = this.fb.group({
      correoElectronico: ['', Validators.required, this.emailValidator.validate.bind(this.emailValidator)],
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      fechaNacimiento: [undefined, Validators.required],
      clave: ['', [Validators.required, Validators.minLength(5)]],
      tipoDocumento: ['TI', Validators.required],
      numeroDocumento: [
        '',
        [Validators.required, Validators.minLength(7), Validators.maxLength(10)],
        this.numDocValidator.validate.bind(this.numDocValidator)
      ]
    });
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
      aplicacionId: 1
    };
    this.userService.crearUsuario(this.userRegister);
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
