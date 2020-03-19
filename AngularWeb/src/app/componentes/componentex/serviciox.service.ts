import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ServicioxService {

  constructor(private http: HttpClient) { }
   ObtenerJson():Observable<any>{
 return this.http.get("http://piedrasdeltunjo.tk/informacion/4")
  }
}
