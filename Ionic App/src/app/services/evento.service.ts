import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Evento } from '../interfaces/evento.interface';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class EventoService {  

  constructor(
    private http: HttpClient
  ) { }

  async buscarEvento(id: number): Promise<Evento> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/eventos/${id}`)
        .subscribe(res => {          
          resolve(res);          
        });
    });
  }

  async leerEventos(): Promise<Evento[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/eventos`)
        .subscribe((res:Evento[]) => {          
          resolve(res);
        });
    });    
  }
}
