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

  getDiasHabilesCabana(id: number): Promise<any[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-cabanas/diasHabiles?id=${ id }`)
                .subscribe(res => {
                  if (res['ok'] === true) {
                    const dates = [];
                    res['reservas'].forEach(x => dates.push(x.split('T')[0]));
                    resolve(dates);
                  } else {
                    resolve([]);
                  }
                });
    });
  }

  getYearValues(dates: any[]) {
    const yearValues = [];
    dates.forEach(x => {
      const year = x.split('-')[0];
      if (!this.existsInArray(year, yearValues)) {
        yearValues.push(year);
      }
    });
    return yearValues;
  }

  getMonthValues(dates: any[]) {
    const monthValues = [];
    dates.forEach(x => {
      const month = x.split('-')[1];
      if (!this.existsInArray(month, monthValues)) {
        monthValues.push(month);
      }
    });
    return monthValues;
  }

  getDayValues(dates: any[], monthV: any) {
    const dayValues = [];
    dates.forEach(x => {
      const month = x.split('-')[1];
      if (month === monthV) {
        const day = x.split('-')[2];
        dayValues.push(day);
      }
    });
    return dayValues;
  }

  existsInArray(value: any, array: any[]) {
    return array.find(x => x === value);
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
