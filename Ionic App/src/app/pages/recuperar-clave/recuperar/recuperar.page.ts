import { Component, OnInit } from '@angular/core';
import { LoadingController, ToastController } from '@ionic/angular';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// Servicios
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-recuperar',
  templateUrl: './recuperar.page.html',
  styleUrls: ['./recuperar.page.scss'],
})
export class RecuperarPage implements OnInit {

  // Variables
  formRecuperarClave: FormGroup;

  constructor(
    private fb: FormBuilder,
    private toastCtrl: ToastController,
    private authService: AuthService,
    private loadingCtrl: LoadingController,
  ) { }

  ngOnInit() {
    this.formRecuperarClave = this.fb.group({            
      correoElectronico: ['', [Validators.required, Validators.minLength(4)]],
      numeroDocumento: ['', [Validators.required, Validators.min(1000000000)]],
    });
  }

  async recuperarClave() {
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();
    console.log(this.formRecuperarClave.get('correoElectronico').value);
    console.log(this.formRecuperarClave.get('numeroDocumento').value);
    const ok = await this.authService.recuperarClave(this.formRecuperarClave.get('correoElectronico').value, String(this.formRecuperarClave.get('numeroDocumento').value));    
    console.log(ok);
    loading.dismiss();
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'bottom',
      duration: 3000
    });
    await toast.present();
  }

  get correoElectronico() {
    return this.formRecuperarClave.get('correoElectronico');
  }

  get numeroDocumento() {
    return this.formRecuperarClave.get('numeroDocumento');
  }
}
