import { Component, OnInit } from '@angular/core';
import { ReservaCabanaService } from 'src/app/services/reserva-cabana.service';
import { ToastController } from '@ionic/angular';
import { CabanaService } from '../../../services/cabana.service';
import { Router } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-reservar',
  templateUrl: './reservar.page.html',
  styleUrls: ['./reservar.page.scss'],
})
export class ReservarPage implements OnInit {

  idCabana = 0;
  valorTotal = 0;
  dates = [];
  yearValues = [];
  monthValues = [];
  dayValues = [];
  nameMonths = ['Ene', 'Feb', 'Mar', 'Abr', 'May',
   'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

  constructor(
    private reservaCabanaService: ReservaCabanaService,
    private toastCtrl: ToastController,
    private router: Router
  ) { }

  ngOnInit() {
    this.reservaCabanaService.changeReservas$.subscribe(_ => this.loadDates());
  }

  async cabanaSelected(event: any) {
    this.idCabana = event.id;
    this.valorTotal = event.valor;
    await this.loadDates();
  }

  async loadDates() {
    // obtener días disponibles de la cabana
    this.dates = await this.reservaCabanaService.getDiasHabilesCabana(this.idCabana);
    this.yearValues = this.reservaCabanaService.getYearValues(this.dates);
    this.monthValues = this.reservaCabanaService.getMonthValues(this.dates);
    this.getDayValuesByMonth(this.monthValues[0]);
  }

  getDayValuesByMonth(month: any) {
    this.dayValues = this.reservaCabanaService.getDayValues(this.dates, month);
  }

  monthChange(event: any) {
    const month = event.target.value;
    this.getDayValuesByMonth(month);
  }

  async reservar() {
    const year = $('#year')[0].value;
    const month = Number($('#month')[0].value) - 1;
    const day = $('#day')[0].value;
    const reserva = {
      fechaReserva: new Date(year, month, day),
      ucabanaId: this.idCabana,
      valorTotal: this.valorTotal
    };
    const created = await this.reservaCabanaService.reservar(reserva);
    if (created) {
      await this.loadDates();
      this.presentToast('Reserva realizada!');
      this.router.navigateByUrl('/cabanas/tus-reservas');
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
      duration: 3000
    });
    toast.present();
  }

  convertNumber(num: string) {
    return Number(num);
  }

}
