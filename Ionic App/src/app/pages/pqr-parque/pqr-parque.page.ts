import { Component, OnInit } from '@angular/core';
import { PqrService } from 'src/app/services/pqr.service';
import { AlertController, ToastController } from '@ionic/angular';
import { Pqr } from '../../interfaces/pqr.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pqr-parque',
  templateUrl: './pqr-parque.page.html',
  styleUrls: ['./pqr-parque.page.scss'],
})
export class PqrParquePage implements OnInit {

  pqrs: Pqr[] = [];

  constructor(
    private pqrService: PqrService,
    private toastCtrl: ToastController,
    private alertCtrl: AlertController,
    private router: Router
  ) { }

  ngOnInit() {
    this.obtenerPqrs();
  }

  async obtenerPqrs() {
    this.pqrs = await this.pqrService.getPqrUser();
    // console.log(this.pqrs);
  }

  async presentAlertPrompt() {
    const alert = await this.alertCtrl.create({
      header: 'Nueva PQR',
      backdropDismiss: false,
      translucent: true,
      inputs: [
        {
          name: 'pregunta',
          type: 'text',
          placeholder: 'Escribe tu pregunta'
        }
      ],
      buttons: [
        {
          text: 'Cancelar',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
            console.log('Cancel clicked!');
          }
        },
        {
          text: 'Enviar',
          handler: (data) => {
            if (data['pregunta'].length > 3) {
              this.agregarPQR(data['pregunta']);
            } else {
              this.presentToast('La pregunta es demasiado corta');
            }
          }
        }
      ]
    });

    await alert.present();
  }

  async presentAlertConfirm(pqr: Pqr) {
    // console.log(pqr);
    const alertConfirm = await this.alertCtrl.create({
      header: 'Alert Confirm',
      message: `¿Estás seguro de eliminar la PQR?`,
      backdropDismiss: false,
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: (blah) => {
            console.log('Cancel');
          }
        },
        {
          text: 'Aceptar',
          handler: () => {
            // console.log('Confirm Okay');
            const deleted = this.pqrService.eliminarPqr(pqr.Id);
            if (deleted) {
              this.pqrs = this.pqrs.filter(x => x.Id !== pqr.Id);
            }
          }
        }
      ]
    });

    await alertConfirm.present();
  }

  agregarPQR(pregunta: string) {
    const pqr: Pqr = {
      FechaPublicacion: new Date(),
      Pregunta: pregunta,
      Respuesta: ''
    };
    // console.log(pqr);
    this.pqrService.agregarPqr(pqr);
    // Refrescar lista
    this.pqrs.unshift(pqr);
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 3000
    });
    await toast.present();
  }

}
