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
export class ServicioeditareService {
private Url: string = 'http://piedrasdeltunjo.tk/Eventos';
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

  
    getusuario(id):Observable<any>{
  return this.http.get(this.Url + id, httpOptions)
  }

  async updateUsuario(cadena,id): Promise<any> {

    //console.log("33  " + cadena.id_tip_doc + " - " + cadena.tipo_documento+ " - " +  cadena.iniciales_tip_doc, this.Url + "/tipdoc")

    return new Promise((resolve, reject) => {
      this.http.put(this.Url+'/'+id, cadena, httpOptions).toPromise()
    });
  }
}
