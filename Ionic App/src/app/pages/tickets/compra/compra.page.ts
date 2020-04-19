import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ReservaTicket } from '../../../interfaces/reserva-ticket.interface';
import { AuthService } from '../../../services/auth.service';
import { ReservaTicketService } from '../../../services/reserva-tickets.service';
import { NavController, ToastController } from '@ionic/angular';
import { Ticket } from '../../../interfaces/ticket';
import {formatDate} from '@angular/common';
@Component({
  selector: 'app-compra',
  templateUrl: './compra.page.html',
  styleUrls: ['./compra.page.scss'],
}) 
export class CompraPage implements OnInit {

  reserva = {
    fechaIngreso: undefined,
    cantidad: 1,
    precio: 0,
    value: " ",
  };


  tickets: Ticket[] = [];
  precioTicket: number;
  precioTicket1: number;
  validar = false;
  validarEdad: boolean = false;
  validarResidencia: boolean = false;
  precioResidente: number;
  precioVisitante: number;
  fechas : Date [] = [];
  min : Date ;
  max : Date ;
  dates = [];
  yearValues = [];
  monthValues = [];
  dayValues = [];
  
  nameMonths = ['Ene', 'Feb', 'Mar', 'Abr', 'May',
   'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

  

  constructor(
    private route: ActivatedRoute,
    private reservaTicketsService: ReservaTicketService,
    private navCtrl: NavController,
    private toastCtrl: ToastController,
    private authService: AuthService,
    private toastCntrl : ToastController,
  ) { }

  ngOnInit() {
    this.validarResidencias();
    this.validarEdades();
    this.obtenerTickets();
    this.obtenerFechasDisponibles();
    this.loadDates();
  }

   async obtenerTickets() {
    try {
      this.reservaTicketsService.obtenerTickets().subscribe(resp => { this.tickets = resp })
    } catch{
    }
  }

  async obtenerFechasDisponibles(){
    this.fechas = await this.reservaTicketsService.obtenerFechasDisponibles();
    this.min = this.fechas[0];
      const id = this.fechas.length;  
      this.max = this.fechas[id-1];
  }

  async validarResidencias() {
    this.validarResidencia = await this.reservaTicketsService.validarResidencia();
  }
  async validarEdades() {
    this.validarEdad = await this.reservaTicketsService.validarEdad();
  }
  async onSubmit(form: NgForm) {
    const id = this.route.snapshot.paramMap.get('id');
    const { fechaIngreso, cantidad  } = form.value;
    const reserva: ReservaTicket = {
      FechaIngreso:fechaIngreso,
      Cantidad: cantidad,
      FechaCompra: fechaIngreso,
      Precio: this.precioTicket,
      idTicket : 1,
    };
    reserva.Qr = '';
    reserva.Token =this.reserva.value;
    if(this.reserva.value=="3"){
      reserva.idTicket = 5;  
      reserva.Cantidad = 1;
    }else if(this.reserva.value=="2"){
      reserva.idTicket = 4;
      reserva.Cantidad = this.reserva.cantidad ;
    }else if(this.reserva.value=="1"){
      reserva.idTicket = 3 ;
      reserva.Cantidad = 1;
    }
    reserva.Precio = this.precioTicket;
    this.reserva.fechaIngreso = formatDate(this.reserva.fechaIngreso,'yyyy/MM/dd','en');
    console.log(this.fechas);
    var validacion = false;
    for(let i = 0 ; i < this.fechas.length; i++){
      if(  formatDate(this.fechas[i],'yyyy/MM/dd','en')==this.reserva.fechaIngreso){
        validacion = true;
      }
    }
    if(validacion==false){
      //no permitir la compra dia cerrado
      this.presentToast('Ups!! Ese dia no atendemos');
    }else{    
    //modulo de pago. // si precio en cero no enviar al modulo de pago.
      const ok = await this.reservaTicketsService.agregarReserva(reserva);
      if(ok == true){
          //mensaje de transaccion exitosa.
          this.presentToast('Transacción exitosa');
          form.reset();
          this.cambiarEstado();
          this.navCtrl.navigateForward('/tickets');
        }else {
          this.presentToast(ok);
        }
    }
    
    
    
  }

  async cambiarEstado(){
    this.validar= false; 
    
  }
  get reservaValida() {
    return this.reserva.fechaIngreso !== undefined && this.reserva.cantidad > 0;
  }

  get precioTotal() {
    return this.reserva.cantidad >= 0 ? (this.reserva.cantidad * this.precioTicket) : 0;
  }

  
  async calcularPrecioTotal() {
    this.precioTicket = this.precioTicket1 * this.reserva.cantidad;
    
  }


  async precios() {
    for (let item = 0; item < this.tickets.length; item++) {
      if (this.tickets[item].Id == 3) {
        this.precioResidente = this.tickets[item].Precio;
      } else if (this.tickets[item].Id == 4) {
        this.precioVisitante = this.tickets[item].Precio;
      }
    }
  }

  async onSelectChange() {
    this.precios();
    this.validar = true;
    if (this.reserva.value == "1") {
      this.precioTicket1 = this.precioResidente;
      this.precioTicket = this.precioResidente;
      
        this.reserva.cantidad = 1;
      
    } else if (this.reserva.value == "2") {
      //ticket visitante
      this.precioTicket1 = this.precioVisitante;
      this.precioTicket = this.precioVisitante;
    } else if (this.reserva.value == "3") {
      
      this.precioTicket1 = 0;
      this.precioTicket = 0;
      this.reserva.cantidad = 1;
    }
  }

    async presentToast(message: any) {
      const toast = await this.toastCntrl.create({
        message,
        position: 'bottom',
        duration: 3500
      });
      await toast.present();
    }


//cargar dias de los tickets
async loadDates() {
  // obtener días disponibles de la cabana
  this.dates = await this.reservaTicketsService.obtenerFechasDisponibles2();
  console.log(this.dates)
  this.yearValues = this.reservaTicketsService.getYearValues(this.dates);
  this.monthValues = this.reservaTicketsService.getMonthValues(this.dates);
  this.getDayValuesByMonth(this.monthValues[0]);
} 

getDayValuesByMonth(month: any) {
  this.dayValues = this.reservaTicketsService.getDayValues(this.dates, month);
}


monthChange(event: any) {
  const month = event.target.value;
  this.getDayValuesByMonth(month);
}




}
