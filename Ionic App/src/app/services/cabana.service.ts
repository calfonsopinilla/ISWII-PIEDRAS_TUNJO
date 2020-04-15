import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Cabana } from '../interfaces/cabana.interface';
import { environment } from '../../environments/environment';
import { catchError } from 'rxjs/operators';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class CabanaService {

  constructor(
    private http: HttpClient
  ) { }

  getCabanas(): Promise<Cabana[]> {
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/cabana`)
                .subscribe((res: Cabana[]) => resolve(res));
    });
  }
}
