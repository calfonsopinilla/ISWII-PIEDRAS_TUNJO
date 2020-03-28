import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';
import { Storage } from '@ionic/storage';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required]],
      clave: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  async login() {
    this.authService.login(this.loginForm.value);
  }

  get correo() {
    return this.loginForm.get('correoElectronico');
  }

  get clave() {
    return this.loginForm.get('clave');
  }

}
