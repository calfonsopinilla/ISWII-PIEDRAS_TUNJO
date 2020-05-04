import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { AuthService } from '../../../services/auth.service';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { NavController, ToastController, ModalController } from '@ionic/angular';
import { Ticket } from '../../../interfaces/ticket.interface';
import { CheckoutPage } from '../../checkout/checkout.page';
import { TicketsService } from '../../../services/tickets.service';
import { Promocion } from '../../../interfaces/promocion.interface';
import { PromocionesService } from '../../../services/promociones.service';


declare var $: any;
@Component({
  selector: 'app-compra',
  templateUrl: './compra.page.html',
  styleUrls: ['./compra.page.scss'],
})

export class CompraPage implements OnInit {

  slidesOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  // ticket
  tickets: Ticket[] = [];
  idSelected = 1;
  ticketSelected: Ticket = {};
  // fecha
  dates = [];
  datesReservas = [];
  yearValues = [];
  monthValues = [];
  dayValues = [];
  nameMonths = ['Ene', 'Feb', 'Mar', 'Abr', 'May',
               'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

  cantidad = 1;
  // actual-promocion
  promocion: Promocion = undefined;
  excentoPago = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastCtrl: ToastController,
    private modalCtrl: ModalController,
    private ticketService: TicketsService,
    private promocionService: PromocionesService,
    private reservaTickService: ReservaTicketService
  ) {}

  ngOnInit() {
    // nuevo login emit
    this.authService.loginState$.subscribe(res => {
      if (res === true) { this.init(); }
    });
    // eliminando reserva emit
    this.reservaTickService.reservaEliminada$.subscribe(res => {
      if (res === true) { this.init(); }
    });
    // normal ngOnInit
    this.init();
  }

  init() {
    this.cargarTiposTickets();
    this.loadDates();
    this.obtenerActualPromocion();
  }

  async cargarTiposTickets() {
    this.tickets = await this.ticketService.getTiposTickets();
    const usuario = await this.authService.getUsuario();
    // console.log(usuario);

    const edad = await this.ticketService.getAgeUser();
    this.excentoPago = (edad < 5 || edad > 65);
    if (this.excentoPago) {
      return;
    }

    if (usuario.LugarExpedicion === 'Facatativa') {
      this.idSelected = 2; // id residente
    } else {
      console.log({edad});
      if (edad >= 5 && edad <= 10) {
        this.idSelected = 3; // id niño 5 - 10 años
      } else {
        this.idSelected = 1; // id visitante
      }
    }
    this.ticketSelected = await this.ticketService.find(this.idSelected);
  }

  async loadDates() {
    // obtener días disponibles de la cabana
    this.dates = await this.reservaTickService.obtenerFechasDisponibles();
    this.yearValues = this.reservaTickService.getYearValues(this.dates);
    this.monthValues = this.reservaTickService.getMonthValues(this.dates);
    this.getDayValuesByMonth(this.monthValues[0]);
  }

  async obtenerActualPromocion() {
    this.promocion = await this.promocionService.obtenerActualPromocion();
    // console.log(this.promocion);
  }

  getDayValuesByMonth(month: any) {
    this.dayValues = this.reservaTickService.getDayValues(this.dates, month);
  }

  monthChange(event: any) {
    const month = event.target.value;
    this.getDayValuesByMonth(month);
  }

  convertNumber(num: string) {
    return Number(num);
  }

  async reservar() {
    const year = $('#year')[0].value;
    const month = Number($('#month')[0].value) - 1;
    const day = $('#day')[0].value;
    // construyecto objeto de reserva de ticket
    const reserva: ReservaTicket = {
      FechaCompra: new Date(),
      FechaIngreso: new Date(year, month, day), // fecha de reserva
      Cantidad: this.cantidad,
      Precio: this.getTotal,
      EstadoId: 1,
      idTicket: this.idSelected
    };
    // console.log(reserva.Precio);

    // modal para el checkout del pago
    const modal = await this.modalCtrl.create({
      component: CheckoutPage,
      componentProps: {
        amount: reserva.Precio
      }
    });
    // finalizar proceso
    await modal.present();
    const { data } = await modal.onDidDismiss();
    // console.log(data);
    if (data) {
      this.presentToast(data.message);
      if (data.ok === true) {
        // agregar la reserva de cabaña
        const created = await this.reservaTickService.agregarReserva(reserva);
        if (created) {
          $('#month')[0].value = this.monthValues[0];
          this.init();
          this.router.navigateByUrl('/tickets/inicio');
        }
      }
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
      duration: 3000
    });
    await toast.present();
  }

  get getSubtotal() {
    return this.ticketSelected.Precio * this.cantidad;
  }

  get getTotal() {
    const total = this.ticketSelected.Precio * this.cantidad;
    let descuento = 0;
    if (this.promocion) {
      descuento = total * (this.promocion.PorcentajeDescuento / 100);
    }
    return total - descuento;
  }

  get array() {
    const arr = [];
    for (let i = 1; i <= 20; i++) { arr.push(i); }
    return arr;
  }
}
