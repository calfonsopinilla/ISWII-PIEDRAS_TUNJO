import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { ReservaCabana } from '../interfaces/reserva-cabana.interface';
import { LoadingController } from '@ionic/angular';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ReservaCabanaService {

  changeReservas$ = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private loadingCtrl: LoadingController
  ) { }

  async reservar(reserva: any): Promise<boolean> {
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    loading.present();

    const usuario = await this.authService.getUsuario();
    reserva['uusuarioId'] = usuario.Id;
    return new Promise(resolve => {
      this.http.post(`${ apiUrl }/reserva-cabanas`, reserva)
                .subscribe(res => {
                  this.changeReservas$.emit(res['ok']);
                  resolve(res['ok']);
                }, err => {}, () => loading.dismiss() );
    });
  }

  async getReservasByUser(): Promise<ReservaCabana[]> {
    const usuario = await this.authService.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-cabanas?userId=${ usuario.Id }`)
               .subscribe(res => {
                 resolve( res['ok'] === true ? res['reservas'] : [] );
               });
    });
  }

  cancelarReservaCabana(id: number) {
    return new Promise(resolve => {
      this.http.delete(`${ apiUrl }/reserva-cabanas/${ id }`)
                .subscribe(res => {
                  this.changeReservas$.emit(res['ok']);
                  resolve(res['ok']);
                });
    });
  }
}
