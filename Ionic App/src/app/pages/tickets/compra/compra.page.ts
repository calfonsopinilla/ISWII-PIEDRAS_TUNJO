import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { AuthService } from '../../../services/auth.service';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { NavController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-compra',
  templateUrl: './compra.page.html',
  styleUrls: ['./compra.page.scss'],
})
export class CompraPage implements OnInit {

  reserva = {
    fechaIngreso: undefined,
    cantidad: 1
  };

  precioTicket = 0;

  constructor(
    private route: ActivatedRoute,
    private reservaTicketsService: ReservaTicketService,
    private navCtrl: NavController,
    private toastCtrl: ToastController,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.obtenerPrecio();
  }

  async obtenerPrecio() {
    this.precioTicket = await this.reservaTicketsService.obtenerPrecio();
  }

  async onSubmit(form: NgForm) {

    const id = this.route.snapshot.paramMap.get('id');
    const { fechaIngreso, cantidad } = form.value;

    const reserva: ReservaTicket = {
      FechaIngreso: fechaIngreso,
      Cantidad: cantidad,
      FechaCompra: new Date(),
      Precio: this.precioTotal
    };
    reserva.Qr = 'http://piedrasdeltunjo.tk/reserva-tickets?userId=' + Number(id) + '&fechaCompra=' + reserva.FechaCompra;
    // console.log(reserva);
    const ok = await this.reservaTicketsService
                          .agregarReserva(reserva);
    if (ok) {
      form.reset();
      this.navCtrl.navigateForward('/tickets');
    }
  }

  get reservaValida() {
    return this.reserva.fechaIngreso !== undefined && this.reserva.cantidad > 0;
  }

  get precioTotal() {
    return this.reserva.cantidad >= 0 ? (this.reserva.cantidad * this.precioTicket) : 0;
  }
}
