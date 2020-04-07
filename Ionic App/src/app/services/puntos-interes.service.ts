import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError } from 'rxjs/operators';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PuntosInteresService {

  constructor(
    private http: HttpClient
  ) { }

  getPuntosInteres() {
    return this.http.get(`${ apiUrl }/puntos-interes`)
                    .pipe(catchError(err => []));
  }
}
