import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { LoadingController, ToastController } from '@ionic/angular';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// Servicios
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-ingresar-codigo',
  templateUrl: './ingresar-codigo.page.html',
  styleUrls: ['./ingresar-codigo.page.scss'],
})
export class IngresarCodigoPage implements OnInit {

  // Variables
  formRecuperarClave: FormGroup;

  constructor(
    private fb: FormBuilder,
    private toastCtrl: ToastController,
    private authService: AuthService,
    private loadingCtrl: LoadingController,
    private router: Router
  ) { }

  ngOnInit() {
    this.formRecuperarClave = this.fb.group({            
      codigoVerificacion: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
      clave: ['', [Validators.required, Validators.minLength(5)]],
      confirmarClave: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  async cambiarClave() {        
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    if (String(this.formRecuperarClave.get('clave').value).localeCompare(String(this.formRecuperarClave.get('confirmarClave').value)) == 0) {
      const res = await this.authService.cambiarClave(this.formRecuperarClave.get('codigoVerificacion').value, this.formRecuperarClave.get('clave').value);
      if (res) {
        this.presentToast("Clave actualizada correctamente");
        this.formRecuperarClave.reset();
        this.router.navigateByUrl('/login');
      } 
    } else {
      this.presentToast("ERROR: Las claves no coinciden");
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

  get codigoVerificacion() {
    return this.formRecuperarClave.get('codigoVerificacion');
  }

  get clave() {
    return this.formRecuperarClave.get('clave');
  }

  get confirmarClave() {
    return this.formRecuperarClave.get('confirmarClave');
  }  
}
