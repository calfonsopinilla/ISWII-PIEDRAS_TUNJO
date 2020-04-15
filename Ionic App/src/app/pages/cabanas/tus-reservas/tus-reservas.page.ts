import { Component, OnInit } from '@angular/core';
import { ReservaCabanaService } from '../../../services/reserva-cabana.service';
import { ReservaCabana } from '../../../interfaces/reserva-cabana.interface';
import { AlertController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-tus-reservas',
  templateUrl: './tus-reservas.page.html',
  styleUrls: ['./tus-reservas.page.scss'],
})
export class TusReservasPage implements OnInit {

  slidesOpts = {
    allowSlideNext: false,
    allowSlidePrev: false
  };

  reservas: ReservaCabana[] = [];

  constructor(
    private reservaCabService: ReservaCabanaService,
    private alertCtrl: AlertController,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
    this.reservaCabService.changeReservas$.subscribe(res => {
      if (res === true) {
        this.obtenerReservas();
      }
    });
    this.obtenerReservas();
  }

  async obtenerReservas() {
    this.reservas = await this.reservaCabService.getReservasByUser();
  }

  async presentAlertConfirm(id: number) {
    const alert = await this.alertCtrl.create({
      header: 'Cancelar reserva',
      message: '<b>¿Estás seguro de cancelar la reserva?</b>',
      buttons: [
        {
          text: 'No',
          role: 'cancel',
          cssClass: '',
          handler: (blah) => {
            console.log('Confirm Cancel: blah');
          }
        },
        {
          text: 'Si, cancelar',
          handler: () => {
            // cancelar reserva cabana
            this.cancelarCabana(id);
          }
        }
      ]
    });
    await alert.present();
  }

  async cancelarCabana(id: number) {
    const deleted = await this.reservaCabService.cancelarReservaCabana(id);
    if (deleted) {
      this.reservas = this.reservas.filter(x => x.Id !== id);
      this.presentToast('Reserva cancelada!');
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
