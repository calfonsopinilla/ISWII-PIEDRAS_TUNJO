import { Injectable, EventEmitter } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { PuntoInteres } from '../../interfaces/punto-interes.interface';
import { tap } from 'rxjs/operators';

const URL = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class PuntosInteresService {

  constructor(
    private http: HttpClient
  ) { }

  getPuntosInteres() {
    return this.http.get<PuntoInteres[]>(`${ URL }/puntos-interes`);
  }

  postPuntosInteres(punto: PuntoInteres) {
    return this.http.post(`${ URL }/puntos-interes`, punto);
  }

  putPuntosInteres(punto: PuntoInteres) {
    return this.http.put(`${ URL }/puntos-interes/${ punto.Id }`, punto);
  }

  deletePuntosInteres(id: number): Promise<boolean> {
    return new Promise(resolve => {
      this.http.delete(`${ URL }/puntos-interes/${ id }`)
              .subscribe(res => {
                resolve(res['ok']);
              });
    });
  }

  existsPunto(lng: number, lat: number): Promise<boolean> {
    return new Promise(resolve => {
      this.getPuntosInteres().subscribe(puntos => {
        const punto = puntos.find(x => x.Longitud === lng && x.Latitud === lat);
        resolve(punto !== undefined);
      });
    });
  }
}
