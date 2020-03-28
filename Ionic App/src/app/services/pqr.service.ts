import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { Pqr } from '../interfaces/pqr.interface';

const URL = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PqrService {

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) { }

  async getPqrUser(): Promise<Pqr[]> {
    // Obtener usuario del localStorage
    const user = await this.auth.getUsuario();
    // console.log(user);
    return new Promise<Pqr[]>(resolve => {
      if (user) {
        this.http.get<Pqr[]>(`${ URL }/pqr?userId=${ user.Id }`)
                  .subscribe(res => {
                    if (res['ok'] === true) {
                      resolve(res['results']);
                    } else {
                      resolve([]);
                    }
                  });
      } else {
        resolve([]);
      }
    });
  }

  async agregarPqr(pqr: Pqr): Promise<boolean> {
    console.log(pqr);
    const user = await this.auth.getUsuario();
    pqr.UUsuarioId = user.Id;
    return new Promise(resolve => {
      if (user) {
        this.http.post(`${ URL }/pqr`, pqr)
                  .subscribe(res => {
                    resolve(res['ok']);
                  });
      } else {
        resolve(false);
      }
    });
  }

  eliminarPqr(id: number): Promise<boolean> {
    return new Promise(resolve => {
      this.http.delete(`${ URL }/pqr/${ id }`)
                .subscribe(res => {
                  resolve(res['ok']);
                });
    });
  }
}
