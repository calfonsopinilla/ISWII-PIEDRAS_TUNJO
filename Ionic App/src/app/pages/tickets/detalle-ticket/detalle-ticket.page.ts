import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { NavController, AlertController, ToastController } from '@ionic/angular';
// import { BarcodeScanner } from '@ionic-native/barcode-scanner/ngx';
// import { Base64ToGallery } from '@ionic-native/base64-to-gallery/ngx';

@Component({
  selector: 'app-detalle-ticket',
  templateUrl: './detalle-ticket.page.html',
  styleUrls: ['./detalle-ticket.page.scss'],
})
export class DetalleTicketPage implements OnInit {

  reserva: ReservaTicket;
  qrCode: string;
  scannedCode = null;
  elementType: 'url' | 'canvas' | 'img' = 'img';

  constructor(
    private route: ActivatedRoute,
    private reservaTicketService: ReservaTicketService,
    private alertCtrl: AlertController,
    private navCtrl: NavController,
    // private barcodeScanner: BarcodeScanner,
    // private base64ToGallery: Base64ToGallery,
    private toastCntrl: ToastController
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.reservaTicketService.buscarReserva(Number(id))
                            .subscribe(res => {
                              this.reserva = res['reserva'];
                              this.qrCode = this.reserva.Qr;
                            });
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
