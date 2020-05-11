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
  private formRecuperarClave: FormGroup;

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
    const ok = await this.authService.recuperarClave(this.formRecuperarClave.get('correoElectronico').value, String(this.formRecuperarClave.get('numeroDocumento').value));        
    if (ok == true) {
      this.presentToast("Datos correctos, el código de verificación se ha generado correctamente");
    } else {
      this.presentToast("ERROR: Ha ocurrido un error inesperado, intentelo de nuevo");
    }
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
