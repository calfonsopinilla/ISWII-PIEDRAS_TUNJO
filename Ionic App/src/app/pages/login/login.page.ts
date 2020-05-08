import { Router } from '@angular/router';
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

  slideOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    // queryString login
    // this.redirect = this.route.snapshot.queryParamMap.get('redirect');
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required, Validators.pattern('[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$')]],
      clave: ['', [Validators.required, Validators.minLength(5)]]
    });

    this.authService.loginState$.subscribe(res => {
      if (res === false) {
        this.loginForm.reset();
      }
    });
  }  

  async login() {
    this.authService.login(this.loginForm.value);
  }

  async olvideClave() {
    this.router.navigate(['/recuperar-clave']);
  }

  get correo() {
    return this.loginForm.get('correoElectronico');
  }

  get clave() {
    return this.loginForm.get('clave');
  }

}
