import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Promocion } from '../interfaces/promocion.interface';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PromocionesService {

  nuevaReservaPromo$ = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient
  ) { }

  async getPromociones(): Promise<Promocion[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/promocion/leer?estado=1`)
                .subscribe(res => {
                  resolve(res['promociones']);
                });
    });
  }

  async obtenerActualPromocion() {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/promocion/actual-promocion`)
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(res['promocion']);
                  } else {
                    resolve(undefined);
                  }
                });
    });
  }
}

