import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { Suscripciones } from '../interfaces/suscripciones.interface'; 

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class SuscripcionService {

  constructor(
    private http: HttpClient
  ) { }

  public leerSuscripciones() {
    return this.http.get(`${ urlApi }/Subscripcion/Ver_Subscripciones?estadoFiltro=1`) 
            .pipe(catchError(err => {
                return of( err.error );
            }));     
  }
}
