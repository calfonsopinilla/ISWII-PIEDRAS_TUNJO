import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Promocion } from '../interfaces/promocion.interface';
import { ReservaPromocion } from '../interfaces/reserva-promocion.interface';
import { AuthService } from './auth.service';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PromocionesService {

  nuevaReservaPromo$ = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getPromociones(): Promise<Promocion[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/promocion/leer?estado=1`)
                .subscribe(res => {
                  resolve(res['promociones']);
                });
    });
  }

  async reservarPromocion(reserva: ReservaPromocion) {
    const usuario = await this.authService.getUsuario();
    reserva.UsuarioId = usuario.Id;
    return new Promise(resolve => {
      this.http.post(`${ apiUrl }/reserva_promocion/crear`, reserva)
                .subscribe(res => {
                  this.nuevaReservaPromo$.emit(true);
                  resolve(res['ok']);
                });
    });
  }

  async getReservasByUserId(): Promise<ReservaPromocion[]> {
    const usuario = await this.authService.getUsuario();

    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva_promocion/leer_usuario?id=${ usuario.Id }`)
                .subscribe(res => {
                  // console.log(res);
                  resolve(res['promociones']);
                });
    });
  }
}

