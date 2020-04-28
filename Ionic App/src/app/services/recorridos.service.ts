import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Storage } from '@ionic/storage';
import { Router } from '@angular/router';
import { Recorrido } from '../interfaces/recorrido.interface';
import { environment } from '../../environments/environment';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class RecorridosService {

  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
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

  async getRecorridos(): Promise<Recorrido[]> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/recorridos`, { headers: this.headers })
                .subscribe(res => {
                  if (res['ok'] === true) {
                    resolve(res['recorridos']);
                  } else {
                    resolve([]);
                  }
                });
    });
  }
}
