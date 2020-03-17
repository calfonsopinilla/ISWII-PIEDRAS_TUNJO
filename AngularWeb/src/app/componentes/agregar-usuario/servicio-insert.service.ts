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
export class ServicioInsertService {
private Url: string = 'http://piedrasdeltunjo.tk/administrador/agregarUsuario';
  constructor(private http: HttpClient) { }
   private extractData(res: Response) {

    let body = JSON.parse('' + res);

    return body || {};
  }
 private handleError<T>(operation = 'operation', result?: T) {

    return (error: any): Observable<T> => {

      console.log(`${operation} failed: ${error.message}`);
      return of(result as T)
    };
  }

  async insertUsuario(usuario): Promise<any> {

 

    return new Promise((resolve, reject) => {
      this.http.post(this.Url , usuario, httpOptions).toPromise()
    });
  }
}
