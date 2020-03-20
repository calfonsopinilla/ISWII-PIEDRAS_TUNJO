import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      correo: ['', [Validators.required]],
      clave: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
    this.loginService.login(this.loginForm.value);
  }

  get correo() {
    return this.loginForm.get('correo');
  }

  get clave() {
    return this.loginForm.get('clave');
  }

}
