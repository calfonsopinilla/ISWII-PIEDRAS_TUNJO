import { Component, OnInit } from '@angular/core';
import { PqrService } from 'src/app/services/pqr.service';
import { AlertController, ToastController } from '@ionic/angular';
import { Pqr } from '../../interfaces/pqr.interface';

@Component({
  selector: 'app-pqr-parque',
  templateUrl: './pqr-parque.page.html',
  styleUrls: ['./pqr-parque.page.scss'],
})
export class PqrParquePage implements OnInit {

  pqrs: Pqr[] = [];
  pqrsA: Pqr[] = [];
  pqrsB: Pqr[] = [];
 
  constructor(
    private pqrService: PqrService,
    private toastCtrl: ToastController,
    private alertCtrl: AlertController
  ) { }

  ngOnInit() {
    this.obtenerPqrs();
  }

  async obtenerPqrs() {
    this.pqrs = await this.pqrService.getPqrUser();
    // console.log(this.pqrs);
    await this.pqrs.forEach(val => {
      if (val["UEstadoPQRId"] == 1) { this.pqrsA.push(Object.assign({}, val)); } else { this.pqrsB.push(Object.assign({}, val)); }
    });
    console.log(this.pqrsA);
  }

  async presentAlert(pqr: Pqr) {
    const alert = await this.alertCtrl.create({
      header: pqr.Pregunta,
      backdropDismiss: true,
      translucent: true,
      message: `<b>Rta: </b>${ pqr.Respuesta || 'No hay respuesta' }`
    });
    await alert.present();
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
          handler: async () => {
            // console.log('Confirm Okay');
            const deleted = await this.pqrService.eliminarPqr(pqr.Id);
            if (deleted) {
              this.pqrs = this.pqrs.filter(x => x.Id !== pqr.Id);
            } else {
              this.presentToast('Ha ocurrido un error');
            }
          }
        }
      ]
    });

    await alertConfirm.present();
  }

  async agregarPQR(pregunta: string) {
    const pqr: Pqr = {
      Id: null,
      FechaPublicacion: new Date(),
      Pregunta: pregunta,
      Respuesta: '',
      FechaRespuesta: null,
      Token: "token",
      LastModification: null,
      UUsuario: null,
      UEstadoPQRId: null,
      UEstadoPQR: null,
    };
        
    const ok = await this.pqrService.agregarPqr(pqr);
    console.log(ok);
    if (ok) {
      // Refrescar lista
      this.pqrsB.unshift(pqr);
    } else {
      this.presentToast('Ha ocurrido un error');
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 3000
    });
    await toast.present();
  }

}
