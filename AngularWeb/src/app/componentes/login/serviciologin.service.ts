import { Injectable ,Inject} from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { IUserInfo } from './user-info';



@Injectable({
  providedIn: 'root'
})
export class ServiciologinService {

    constructor(private http: HttpClient  ) { }
	// private baseurl:"http://piedrasdeltunjo.tk/usuario/iniciaSesion";

 ObtenerJson():Observable<any>{
 return this.http.get("http://piedrasdeltunjo.tk/administrador/informacionUsuarios")
  }


  


}
