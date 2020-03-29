import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { ReservaTicket } from '../interfaces/reserva-ticket.interface';
import { UserLogin } from '../interfaces/user-login.interface';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ReservaTicketService {

  nuevaReserva$ = new EventEmitter<ReservaTicket>();
  reservaEliminada$ = new EventEmitter<number>();

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) { }

  async getTicketsUser(): Promise<ReservaTicket[]> {
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
      if (user) {
        this.http.get(`${ apiUrl }/reserva-tickets?userId=${ user.Id }`)
                  .subscribe(res => {
                    if (res['ok'] === true) {
                      resolve(res['results']);
                    } else {
                      resolve([]);
                    }
                  });
      } else {
        return resolve([]);
      }
    });
  }

  async agregarReserva(reserva: ReservaTicket): Promise<boolean> {
    const user = await this.auth.getUsuario();
    reserva.UUsuarioId = user.Id;
    return new Promise(resolve => {
      if (user) {
        this.http.post(`${ apiUrl }/reserva-tickets`, reserva)
                  .subscribe(res => {
                    this.nuevaReserva$.emit(reserva);
                    resolve(res['ok']);
                  });
      } else {
        resolve(false);
      }
    });
  }

  buscarReserva(id: number) {
    return this.http.get(`${ apiUrl }/reserva-tickets/${ id }`);
  }

  eliminarReserva(id: number): Promise<boolean> {
    return new Promise(resolve => {
        this.http.delete(`${ apiUrl }/reserva-tickets/${ id }`)
                  .subscribe(res => {
                    this.reservaEliminada$.emit(id);
                    resolve(res['ok']);
                  });
    });
  }


}
