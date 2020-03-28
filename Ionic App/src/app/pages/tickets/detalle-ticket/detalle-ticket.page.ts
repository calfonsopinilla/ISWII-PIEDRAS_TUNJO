import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { NavController, AlertController } from '@ionic/angular';

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
    private navCtrl: NavController
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.reservaTicketService.buscarReserva(Number(id))
                            .subscribe(res => {
                              this.reserva = res['reserva'];
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

}
