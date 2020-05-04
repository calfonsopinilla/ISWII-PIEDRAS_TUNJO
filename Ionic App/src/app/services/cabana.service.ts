import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Cabana } from '../interfaces/cabana.interface';
import { environment } from '../../environments/environment';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Storage } from '@ionic/storage';
import { of, pipe } from 'rxjs';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class CabanaService {

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

  async getCabanas(): Promise<Cabana[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/cabana`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
                .subscribe((res: Cabana[]) => resolve(res || []));
    });
  }

  async getCabana(id: number): Promise<Cabana> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve(undefined);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/cabana/${ id }`, { headers: this.headers })
                .pipe(
                  catchError(err => of({ ok: false }))
                )
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(res['cabana']);
                  } else {
                    resolve(undefined);
                  }
                });
    });
  }
}
