import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Storage } from '@ionic/storage';
import { Router } from '@angular/router';
import { Recorrido } from '../interfaces/recorrido.interface';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class RecorridosService {

  constructor(
    private http: HttpClient
  ) { }

  async getRecorridos(): Promise<Recorrido[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/recorridos`)
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
