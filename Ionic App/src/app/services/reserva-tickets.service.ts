import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { ReservaTicket } from '../interfaces/reserva-ticket.interface';
import { UserLogin } from '../interfaces/user-login.interface';
import { tap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import {Ticket} from '../interfaces/ticket.interface';
import { Router } from '@angular/router';
import { Storage } from '@ionic/storage';
const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ReservaTicketService {

  private headers: HttpHeaders;
  nuevaReserva$ = new EventEmitter<ReservaTicket>();
  reservaEliminada$ = new EventEmitter<number>();

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private storage: Storage,
    private router: Router
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

  async transferirTicket(id: number, numeroDocumento: string, cantidadTransferir: number): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/transferir?id=${ id }&numeroDocumento=${ numeroDocumento }&cantidadTransferir=${ cantidadTransferir }`, { headers: this.headers })
        .pipe(          
          catchError(err => of({ok: false}))
        )
        .subscribe(res => {          
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        });
    });
  }

  async getTicketsUser(): Promise<ReservaTicket[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
      if (user) {
        this.http.get(`${ apiUrl }/reserva-tickets?userId=${ user.Id }`, { headers: this.headers })
                  .pipe(
                    catchError(err => of({ok: false}))
                  )
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
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(false);
    }
    const user = await this.auth.getUsuario();
    reserva.UUsuarioId = user.Id;
    reserva.NumeroDocumento = user.NumeroDocumento;
    return new Promise(resolve => {
      if (user) {
        this.http.post(`${ apiUrl }/reserva-tickets/crear`, reserva, { headers: this.headers })
                  .pipe(
                    catchError(err => of({ok: false}))
                  )
                  .subscribe(res => {
                    this.nuevaReserva$.emit(reserva);
                    resolve(res['ok']);
                  });
      } else {
        resolve(false);
      }
    });
  }

  async buscarReserva(id: number) : Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(false);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/${ id }`, { headers: this.headers })
        .subscribe(res => {          
          resolve(res['reserva']);
        }, err => {
          resolve(false);
        });
    });
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

  async obtenerPrecio(): Promise<number> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(0);
    }
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/obtenerPrecio?userId=${ user.Id }`, { headers: this.headers })
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(Number(res['precio']));
                  } else {
                    resolve(-1);
                  }
                });
    });
  }

  async leerReservaToken(qr: string): Promise<ReservaTicket> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(undefined);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerToken?qr=${ qr }`, { headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(res['reserva']);
          } else {
            resolve(null);
          }
        });
    });
  } 

  /* Jhonattan Pulido */
  async leerQr(qr: string): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerToken?qr=${ qr }`, { headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(res['reserva']);
          }
        }, (error) => {
          resolve(false);
        });
    });
  }

  /* Jhonattan Pulido */
  async leerDNI(numeroDocumento: string): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerDNI?numeroDocumento=${ numeroDocumento }`, { headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(res['reserva']);
          }
        }, (error) => {
          resolve(false);
        });
    });
  }

  /* Jhonattan Pulido */
  async validarQr(id: number): Promise<boolean> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(false);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/validarQr?id=${ id }`, { headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          }
        }, (error) => {
          resolve(false);
        });
    });
  }

  obtenerTicket( id: number): Observable<Ticket> {
      return this.http.get<Ticket>(`${ apiUrl }/tickets/${id}`);
  }
  obtenerTickets(): Observable<Ticket[]>{
    return this.http.get<Ticket[]>(`${ apiUrl }/tickets`);
  }

  async obtenerFechasDisponibles(): Promise<any[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
        return this.http.get(`${ apiUrl }/reserva-tickets/validarFechas?userId=${ user.Id }`, { headers: this.headers })
                  .subscribe(res => {
                    if (res['ok'] === true) {
                      const dates = [];
                      res['results'].forEach(x => dates.push(x.split('T')[0]));
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

}
