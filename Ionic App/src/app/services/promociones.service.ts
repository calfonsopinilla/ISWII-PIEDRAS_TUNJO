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
                  console.log(res);
                  resolve(res['promociones']);
                });
    });
  }

  // async reservarPromocion(reserva: ReservaPromocion) {
  //   const prepare = await this.prepareHeaders();
  //   if (!prepare) {
  //     console.log('token not found');
  //     return Promise.resolve([]);
  //   }
  //   const usuario = await this.authService.getUsuario();
  //   reserva.UsuarioId = usuario.Id;
  //   return new Promise(resolve => {
  //     this.http.post(`${ apiUrl }/reserva_promocion/crear`, reserva, { headers: this.headers })
  //               .pipe(
  //                 catchError(err => of({ ok: false }))
  //               )
  //               .subscribe(res => {
  //                 this.nuevaReservaPromo$.emit(true);
  //                 resolve(res['ok']);
  //               });
  //   });
  // }

  // async getReservasByUserId(): Promise<ReservaPromocion[]> {
  //   const prepare = await this.prepareHeaders();
  //   if (!prepare) {
  //     console.log('token not found');
  //     return Promise.resolve([]);
  //   }
  //   const usuario = await this.authService.getUsuario();
  //   return new Promise(resolve => {
  //     this.http.get(`${ apiUrl }/reserva_promocion/leer_usuario?id=${ usuario.Id }`, { headers: this.headers })
  //               .pipe(
  //                 catchError(_ => of({ promociones: [] }))
  //               )
  //               .subscribe(res => {
  //                 console.log();
  //                 resolve(res['promociones']);
  //               });
  //   });
  // }
}

