import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { ModalController, NavController, AlertController, ToastController } from '@ionic/angular';
import { TransferirTicketPage } from '../../transferir-ticket/transferir-ticket.page';

@Component({
  selector: 'app-detalle-ticket',
  templateUrl: './detalle-ticket.page.html',
  styleUrls: ['./detalle-ticket.page.scss'],
})
export class DetalleTicketPage implements OnInit {

  reserva: ReservaTicket;    

  constructor(
    private route: ActivatedRoute,
    private reservaTicketService: ReservaTicketService,
    private alertCtrl: AlertController,
    private navCtrl: NavController,    
    private toastCntrl: ToastController,
    private modalCtrl: ModalController
  ) { }

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    const res = await this.reservaTicketService.buscarReserva(Number(id));
    this.reserva = res;    
  }

  async presentAlertConfirm() {
    const alertConfirm = await this.alertCtrl.create({
      header: 'Alert Confirm',
      message: '¿Estás seguro de cancelar esta reserva?',
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
          text: 'Okay',
          handler: () => {
            // console.log('Confirm Okay');
            this.eliminarReserva();
          }
        }
      ]
    });

    await alertConfirm.present();
  }

  async eliminarReserva() {
    const id = this.route.snapshot.paramMap.get('id');
    const ok = await this.reservaTicketService.eliminarReserva(Number(id));
    if (ok) {
      this.navCtrl.navigateForward('/tickets');
    }
  }

  async transferirTicket() {
    // modal para el checkout del pago
    const modal = await this.modalCtrl.create({
      component: TransferirTicketPage,
      componentProps: {
        id: Number(this.route.snapshot.paramMap.get('id')),
        cantidad: (this.reserva.Cantidad-1),
        numeroDocumentoUsuario: (this.reserva.NumeroDocumento)
      }
    });
    await modal.present();
    const { data } = await modal.onDidDismiss();    
    if (Boolean(data)) {
      const id = this.route.snapshot.paramMap.get('id');
      const res = await this.reservaTicketService.buscarReserva(Number(id));
      this.reserva = res;
    }
  }

  // async downloadQrCode() {
  //   const canvas = document.querySelector('canvas');
  //   const imageData = canvas.toDataURL('image/jpeg').toString();
  //   let data = imageData.split(',')[1];
  //   this.base64ToGallery.base64ToGallery(
  //     data,
  //     {prefix: '_img', mediaScanner: true}
  //     ).then(async res => {
  //       let toast = await this.toastCntrl.create({
  //         header: 'QR Code guardado en la galería de imágenes'
  //       });
  //     }, async err => {
  //       let toast = await this.toastCntrl.create({
  //         header: 'ERROR: No se ha almacenado la imagen'
  //       });
  //     }
  //     );
  // }
}
