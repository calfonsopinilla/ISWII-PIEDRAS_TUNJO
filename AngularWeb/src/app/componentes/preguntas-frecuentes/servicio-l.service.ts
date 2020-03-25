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

export class ServicioLService {
private Url: string = 'http://piedrasdeltunjo.tk/preguntasfrecuentes';
   constructor(private http: HttpClient) { }

  ObtenerJson():Observable<any>{

 return this.http.get(this.Url)

  }

   getu(id):Observable<any>{
  return this.http.get(this.Url + id, httpOptions)
  }
  async update(cadena,id): Promise<any> {

    return new Promise((resolve, reject) => {
      this.http.put(this.Url+'/'+id, cadena, httpOptions).toPromise()
    });
  }

async Eliminar(id): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.delete(this.Url+'/'+id).toPromise()
    });
  }

async insertar(Datos): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.post(this.Url , Datos, httpOptions).toPromise()
    });
  }


 }



  /*async updateUsuario(cadena,id): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.put(this.Url+'/'+id, cadena, httpOptions).toPromise()
    });
}
}*/



