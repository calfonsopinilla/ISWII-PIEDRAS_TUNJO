import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
    private authService: AuthService,
    private navCtrl: NavController,
    private toastCtrl: ToastController,
    private storage: Storage,
    private loadingCtrl: LoadingController
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      correoElectronico: ['', [Validators.required]],
      clave: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  async login() {
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();
    this.authService.login(this.loginForm.value)
                    .subscribe(
                      async (res) => {
                        setTimeout(_ => {}, 5000);
                        if (res['ok'] === true) {
                          this.storage.clear();
                          const ok = await this.storage.set('usuario', res['userLogin']);
                          console.log(ok);
                          if (ok) {
                            this.navCtrl.navigateRoot('/inicio');
                          }
                        } else {
                          this.presentToast(res['message']);
                        }
                      },
                      (err) => {},
                      () => loading.dismiss()
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

  get correo() {
    return this.loginForm.get('correoElectronico');
  }

  get clave() {
    return this.loginForm.get('clave');
  }

}
