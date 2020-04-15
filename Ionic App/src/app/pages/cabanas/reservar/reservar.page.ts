import { Component, OnInit } from '@angular/core';
import { ReservaCabanaService } from 'src/app/services/reserva-cabana.service';
import { ToastController } from '@ionic/angular';
import { CabanaService } from '../../../services/cabana.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reservar',
  templateUrl: './reservar.page.html',
  styleUrls: ['./reservar.page.scss'],
})
export class ReservarPage implements OnInit {

  idCabana = 0;
  valorTotal = 0;
  fecha = undefined;

  constructor(
    private reservaCabanaService: ReservaCabanaService,
    private toastCtrl: ToastController,
    private router: Router
  ) { }

  ngOnInit() {
    // console.log(this.fecha);
  }

  async cabanaSelected(event: any) {
    this.idCabana = event.id;
    this.valorTotal = event.valor;
  }

  async reservar() {
    const reserva = {
      fechaReserva: this.fecha,
      ucabanaId: this.idCabana,
      valorTotal: this.valorTotal
    };
    const created = await this.reservaCabanaService.reservar(reserva);
    if (created) {
      this.presentToast('Reserva realizada!');
      this.router.navigateByUrl('/cabanas/tus-reservas');
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 3000
    });
    toast.present();
  }

}
