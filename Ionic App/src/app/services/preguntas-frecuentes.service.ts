import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FrequentQuestion } from '../interfaces/frequent-questions.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PreguntasFrecuentesService {

  // Constructor
  constructor(
    private http: HttpClient
  ) { }

  leerPreguntasFrecuentes() {
    return this.http.get(`${ urlApi }/preguntasfrecuentes`)
            .pipe(catchError(err => {
                return of( err.error );
            }));
  }
}
