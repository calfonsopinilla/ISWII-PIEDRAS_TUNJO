import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Promocion } from '../interfaces/promocion.interface';
import { ReservaPromocion } from '../interfaces/reserva-promocion.interface';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { Storage } from '@ionic/storage';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PromocionesService {

  nuevaReservaPromo$ = new EventEmitter<boolean>();
  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private storage: Storage
  ) { }

  async prepareHeaders() {
    const token = await this.storage.get('token') || undefined;
    if (!token) {
      this.router.navigateByUrl('/login');
      return false;
    } else {
      this.headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      return true;
    }
  }

  async getPromociones(): Promise<Promocion[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/promocion/leer?estado=1`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
                .subscribe(res => {
                  // console.log(res);
                  resolve(res['promociones']);
                });
    });
  }

  async obtenerActualPromocion() {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(undefined);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/promocion/actual-promocion`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
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

