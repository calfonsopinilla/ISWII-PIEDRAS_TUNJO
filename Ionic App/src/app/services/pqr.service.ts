import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { Pqr } from '../interfaces/pqr.interface';
import { Storage } from '@ionic/storage';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { OneSignalService } from './one-signal.service';

const URL = environment.servicesAPI;
const redirectUrl = 'http://piedras-tunjo.herokuapp.com/pqr';

@Injectable({
  providedIn: 'root'
})
export class PqrService {

  private user: any;
  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private storage: Storage,
    private oneSignalService: OneSignalService,
    private router: Router
  ) { }

  async buildService() {
    // Obtener usuario
    this.user = await this.auth.getUsuario();
    // Obtener token
    const token = await this.storage.get('token') || undefined;
    if (!token || !this.user) {
      this.router.navigateByUrl('/login');
      return false;
    } else {
      this.headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      return true;
    }
  }

  async getPqrUser(): Promise<Pqr[]> {
    const build = await this.buildService();
    // console.log(build);
    if (!build) {
      console.log('build service error');
      return Promise.resolve([]);
    }
    return new Promise<Pqr[]>(resolve => {
      this.http.get<Pqr[]>(`${ URL }/pqr?userId=${ this.user.Id }`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(res['results']);
                  } else {
                    resolve([]);
                  }
                });
    });
  }

  async agregarPqr(pqr: Pqr): Promise<boolean> {
    const build = await this.buildService();
    if (!build) { return Promise.resolve(false); }
    pqr.UUsuarioId = this.user.Id;
    return new Promise(resolve => {
      this.http.post(`${ URL }/pqr`, pqr, { headers: this.headers })
                .pipe(
                  catchError(err =>  of({ ok: false }))
                )
                .subscribe(res => {
                  if (res['ok'] === true) {
                    this.oneSignalService.sendNotification('Nuevo PQR de usuario', redirectUrl);
                  }
                  resolve(res['ok']);
                });
    });
  }

  async eliminarPqr(id: number): Promise<boolean> {
    const build = await this.buildService();
    if (!build) { return Promise.resolve(false); }

    return new Promise(resolve => {
      this.http.delete(`${ URL }/pqr/${ id }`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
                .subscribe(res => {
                  resolve(res['ok']);
                });
    });
  }
}
