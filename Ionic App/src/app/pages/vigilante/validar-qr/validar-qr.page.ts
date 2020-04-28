import { Component, OnInit } from '@angular/core';
import { AlertController, ToastController } from '@ionic/angular';
import { BarcodeScanner } from '@ionic-native/barcode-scanner/ngx';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface'; 
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-validar-qr',
  templateUrl: './validar-qr.page.html',
  styleUrls: ['./validar-qr.page.scss'],
})
export class ValidarQrPage implements OnInit {

  spend: Boolean;
  reservaTicket: ReservaTicket;
  scannedCode = null;

  constructor(
    private barCodeScanner: BarcodeScanner,
    private reservaTicketService: ReservaTicketService,
    private toastCtrl: ToastController,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  async logout() {
    await this.authService.logout();
  }

  async scanCode() {
    await this.barCodeScanner.scan().then(barcodeData => {
      this.scannedCode = barcodeData.text;            
    });
    this.reservaTicket = await this.reservaTicketService.leerReservaToken(this.scannedCode);
    if (this.reservaTicket == null) {
      this.presentToast("No se encontro la reserva");
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
