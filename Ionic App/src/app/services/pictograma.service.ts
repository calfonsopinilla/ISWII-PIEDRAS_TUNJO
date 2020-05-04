import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Pictograma } from '../interfaces/pictograma.interface';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PictogramaService {

  constructor(
    private http: HttpClient
  ) { }

  async getPictogramas(): Promise<Pictograma[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/pictogramas`)
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
