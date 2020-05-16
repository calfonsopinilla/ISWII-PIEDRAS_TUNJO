import { Component, OnInit } from '@angular/core';
import { AlertController, ToastController, MenuController } from '@ionic/angular';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface'; 
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-validar-numero-documento',
  templateUrl: './validar-numero-documento.page.html',
  styleUrls: ['./validar-numero-documento.page.scss'],
})
export class ValidarNumeroDocumentoPage implements OnInit {

  spend: Boolean;
  reservaTicket: ReservaTicket;
  dni: string;

  constructor(
    private reservaTicketService: ReservaTicketService,
    private toastCtrl: ToastController,
    private authService: AuthService,
    private menuCtrl: MenuController
  ) { }

  ngOnInit() {
    this.menuCtrl.enable(false, 'first');
    this.reservaTicket = null;
  }

  async logout() {
    this.menuCtrl.enable(true, 'first');
    await this.authService.logout();
  }

  async readDNI() {
    const reserva = await this.reservaTicketService.leerDNI(this.dni);        
    if (reserva != false) {
      this.reservaTicket = reserva;
    } else {
      this.presentToast("No se encontro la reserva");
      this.reservaTicket = null;
    }
  }

  async spendCode() {
    this.spend = await this.reservaTicketService.validarQr(this.reservaTicket.Id);
    if (this.spend == true) {
      this.presentToast("Validación exitosa");
      this.reservaTicket = null;
    } else {
      this.presentToast("ERROR: Intentelo de nuevo");
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
