import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';
import { Storage } from '@ionic/storage';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm: FormGroup;
  redirect = 'no';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private loadingCtrl: LoadingController
  ) { }

  ngOnInit() {
    // queryString login
    // this.redirect = this.route.snapshot.queryParamMap.get('redirect');

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
