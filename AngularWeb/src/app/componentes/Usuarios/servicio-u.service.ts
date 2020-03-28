import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
const httpOptions =
{
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class ServicioUService {

  constructor(private http: HttpClient) { }
private url: string = "http://piedrasdeltunjo.tk/Usuarios";
  ObtenerJson():Observable<any>{
 return this.http.get(this.url)
  }
  

getu(id):Observable<any>{
  return this.http.get(this.url + id, httpOptions)
  }
  async update(cadena,id): Promise<any> {

    return new Promise((resolve, reject) => {
      this.http.put(this.url+'/'+id, cadena, httpOptions).toPromise()
    });
  }

async Eliminar(id): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.delete(this.url+'/'+id).toPromise()
    });
  }

async insertar(Datos): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.post(this.url , Datos, httpOptions).toPromise()
    });
  }


}
