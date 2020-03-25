import { Component, OnInit } from '@angular/core';
import { PqrService } from 'src/app/services/pqr.service';
import { AlertController, ToastController } from '@ionic/angular';
import { PQR } from 'src/app/interfaces/pqr.interface';

@Component({
  selector: 'app-pqr-parque',
  templateUrl: './pqr-parque.page.html',
  styleUrls: ['./pqr-parque.page.scss'],
})
export class PqrParquePage implements OnInit {

  items: any[] = [];

  constructor(
    private pqrService: PqrService,
    private toastCtrl: ToastController,
    private alertCtrl: AlertController
  ) { }

  ngOnInit() {
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
            if (data['pregunta'].length > 6) {
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

  agregarPQR(pregunta: string) {
    const pqr = {
      fechaPublicacion: new Date(),
      pregunta,
      respuesta: '',
      estadoId: 2,
      estado: false
    };
    console.log(pqr);
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 3000
    });
    await toast.present();
  }

}
