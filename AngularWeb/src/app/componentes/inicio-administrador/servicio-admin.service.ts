import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ServicioAdminService {

  constructor(private http: HttpClient) { }
  ObtenerJson():Observable<any>{
 //return this.http.get("http://piedrasdeltunjo.tk/administrador/informacionUsuarios")
 return this.http.get("http://piedrasdeltunjo.tk/Usuarios")
 //return this.http.get("http://localhost:61629/Usuarios")
  }
}
