import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Storage } from '@ionic/storage';
import { Router } from '@angular/router';

import { Comentario } from '../interfaces/comentario.interface';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ComentarioService {

  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private storage: Storage,
    private router: Router
  ) { }

  async crearComentario(table: string, objectId: number, comentario: Comentario): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    console.log(comentario);
    return new Promise(resolve => {      
      this.http.post(`${ apiUrl }/puntuacion/crear?table=${ table }&objectId=${ objectId }`, comentario, { headers: this.headers })
        .subscribe(res => {
          console.log(res);
          if (res['ok'] === true) {
            resolve(true);            
          } else {
            resolve(false);
          }
        }, (err) => {
          console.log(err);
          resolve(null);
        });
    });    
  }

  async actualizarComentario(table: string, objectId: number, comentario: Comentario): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.post(`${ apiUrl }/puntuacion/actualizar?table=${ table }&objectId=${ objectId }`, comentario,{ headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        }, (err) => {
          console.log(err);
          resolve(null);
        });
    });    
  }

  async leerComentarioUsuario(table: string, objectId: number, userId: number): Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/puntuacion/leer-usuario?table=${ table }&objectId=${ objectId }&userId=${ userId }`, { headers: this.headers })
        .subscribe(res => {          
          if (res['ok'] === true) {            
            resolve(res['comentario']);
          } else {
            resolve(null);
          }
        }, (err) => {
          resolve(false);
        });
    });
  }

  async leerComentariosId(table: string, objectId: number) : Promise<any> {
    const prepare = await this.prepareHeaders();
    if (!prepare) {
      console.log('token not found');
      return Promise.resolve([]);
    }
    return new Promise(resolve => {
      this.http.get(`${ apiUrl }/puntuacion/leer?table=${ table }&objectId=${ objectId }`, { headers: this.headers })
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(res['lista']);
          }
        }, (err) => {
          resolve(false);
        });
    });           
  }

  async prepareHeaders() {
    const token = await this.storage.get('token') || undefined;
    if (!token) {
      this.router.navigateByUrl('/login');
      return false;
    } else {
      this.headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      return true;
    }
  }
}
