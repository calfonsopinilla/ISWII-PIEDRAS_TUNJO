import { Component, OnInit } from '@angular/core';
import { BarcodeScanner } from '@ionic-native/barcode-scanner/ngx';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface'; 
import { ReservaTicketService } from '../../../services/reserva-tickets.service';

@Component({
  selector: 'app-validar-qr',
  templateUrl: './validar-qr.page.html',
  styleUrls: ['./validar-qr.page.scss'],
})
export class ValidarQrPage implements OnInit {

  reservaTicket: ReservaTicket;
  scannedCode = null;

  constructor(
    private barCodeScanner: BarcodeScanner,
    private reservaTicketService: ReservaTicketService
  ) { }

  ngOnInit() {
  }

  scanCode() {
    this.barCodeScanner.scan().then(barcodeData => {
      this.scannedCode = barcodeData.text;      
      this.reservaTicketService.leerReservaToken(barcodeData.text)
          .subscribe((resp: ReservaTicket) => this.reservaTicket = resp);
    });
  }

}
