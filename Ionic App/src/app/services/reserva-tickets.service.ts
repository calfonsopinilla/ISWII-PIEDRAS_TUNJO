import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { ReservaTicket } from '../interfaces/reserva-ticket.interface';
import { UserLogin } from '../interfaces/user-login.interface';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {Ticket} from '../interfaces/ticket.interface';
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
        this.http.post(`${ apiUrl }/reserva-tickets/crear`, reserva)
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

  async obtenerPrecio(): Promise<number> {
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/obtenerPrecio?userId=${ user.Id }`)
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(Number(res['precio']));
                  } else {
                    resolve(-1);
                  }
                });
    });
  }

  leerReservaToken(qr: string) : Promise<ReservaTicket> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerToken?qr=${ qr }`)
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
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerToken?qr=${ qr }`)
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
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/leerDNI?numeroDocumento=${ numeroDocumento }`)
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
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/validarQr?id=${ id }`)
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          }
        }, (error) => {
          resolve(false);
        });
    });
  }

  async validarResidencia():Promise<boolean> {
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/validarResidencia?userId=${ user.Id }`)
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(Boolean(res['residencia']));
                  } else {
                    resolve(false);
                  }
                });
    });
}

async validarEdad():Promise<boolean> {
  const user = await this.auth.getUsuario();
  return new Promise(resolve => {
    this.http.get(`${ apiUrl }/reserva-tickets/validarEdad?userId=${ user.Id }`)
              .subscribe(res => {
                if (res['ok'] === true) {
                  resolve(Boolean(res['edad']));
                } else {
                  
                }
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
    const user = await this.auth.getUsuario();
    return new Promise(resolve => {
        return this.http.get(`${ apiUrl }/reserva-tickets/validarFechas?userId=${ user.Id }`)
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
 
 obtenerFechasDisponibles2(): Promise<any[]> {
  return new Promise(resolve => {
    this.http.get(`${ apiUrl }/reserva-tickets/validarFechas`)
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

async obtenerFechasDisponiblesPorTicket(idTicket : number): Promise<any[]> {
  const user = await this.auth.getUsuario();
  return new Promise(resolve => {
    this.http.get(`${ apiUrl }/reserva-tickets/validarFechasUser?idUser=${user.Id}&idTicket=${idTicket}`)
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
