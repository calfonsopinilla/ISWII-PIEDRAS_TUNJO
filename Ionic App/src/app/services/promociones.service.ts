import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Promocion } from '../interfaces/promocion';

const servicesApi = environment.servicesAPI;


@Injectable({
  providedIn: 'root'
})
export class PromocionesService {

  constructor(   private http: HttpClient ) { }
  obtenerInformacionPromociones(): Observable<Promocion[]> {
    return this.http.get<Promocion[]>(`${ servicesApi }/promocion`);
  }


  
}
