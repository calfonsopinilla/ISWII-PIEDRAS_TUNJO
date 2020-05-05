import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Ticket } from '../interfaces/ticket.interface';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Storage } from '@ionic/storage';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class TicketsService {

  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
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
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(-1);
    }
    const user = await this.authService.getUsuario();
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/reserva-tickets/getAgeUser?userId=${ user.Id }`, { headers: this.headers })
              .pipe(
                catchError(err => of({ok: false}))
              )
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
