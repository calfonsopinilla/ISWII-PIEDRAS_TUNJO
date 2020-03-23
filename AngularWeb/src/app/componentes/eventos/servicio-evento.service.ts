import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ServicioEventoService {
constructor(private http: HttpClient) { }
 //private url: string = "http://localhost:61629/Eventos";
  ObtenerJson():Observable<any>{
 return this.http.get("http://piedrasdeltunjo.tk/Eventos/16")
  }


}
