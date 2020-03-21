import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../services/login.service';
import { NavController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private navCtrl: NavController,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required]],
      clave: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  async login() {
    const logged = await this.loginService.login(this.loginForm.value);
    if (logged) {
      this.navCtrl.navigateRoot('/inicio');
    } else {
      this.presentToast('Error message');
    }
  }

  get correo() {
    return this.loginForm.get('correoElectronico');
  }

  get clave() {
    return this.loginForm.get('clave');
  }

  async presentToast(message: string){
    const toast = await this.toastCtrl.create({
      message,
      duration: 2000
    });
    await toast.present();
  }

}
