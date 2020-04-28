import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Pictograma } from '../interfaces/pictograma.interface';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Storage } from '@ionic/storage';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PictogramaService {

  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
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

  async getPictogramas(): Promise<Pictograma[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/pictogramas`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ok: false}))
                )
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(res['pictogramas']);
                  } else {
                    resolve([]);
                  }
                });
    });
  }
}
