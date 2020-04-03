import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { Noticias} from '../interfaces/noticias';



const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ServicioNoticiasService {

  constructor(   private http: HttpClient) { }
  
  obtenerInformacionNoticias(): Observable<Noticias[]> {
    return this.http.get<Noticias[]>(`${ urlApi }/comentariosNoticia/noticias`)
  }

  obtenerNoticiaVer(id): Observable <Noticias>{
  
    return this.http.get<Noticias>(`${ urlApi }/comentariosNoticia/buscarNoticia1?id=${id}`)
  
  }
}
