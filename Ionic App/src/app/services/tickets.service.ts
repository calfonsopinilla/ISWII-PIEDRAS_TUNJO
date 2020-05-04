import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Ticket } from '../interfaces/ticket.interface';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class TicketsService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getTiposTickets(): Promise<Ticket[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/tickets`)
                .subscribe((res: Ticket[]) => resolve(res));
    });
  }

  find(id: number): Promise<Ticket> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/tickets/${ id }`)
              .subscribe(res => {
                resolve(res);
              });
    });
  }

  async getAgeUser(): Promise<number> {
    const user = await this.authService.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/getAgeUser?userId=${ user.Id }`)
              .subscribe(res => {
                if (res['ok'] === true) {
                  resolve(res['edad']);
                } else {
                  resolve(-1);
                }
              });
    });
  }
}
