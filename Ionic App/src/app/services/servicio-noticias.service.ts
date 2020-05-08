import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { Noticias } from '../interfaces/noticias';
import { map, catchError } from 'rxjs/operators';
import { Usuario } from '../interfaces/usuario.interface';
import { Router } from '@angular/router';
import { Storage } from '@ionic/storage';
import { async } from '@angular/core/testing';
import { LoadingController, ToastController } from '@ionic/angular';
const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ServicioNoticiasService {

  usuario: Usuario;
  constructor(private http: HttpClient,
    private router: Router,
    private storage: Storage,
    private toastCtrl: ToastController,
    private loadingCtrl: LoadingController
  ) { }


  async obtenerNoticias(): Promise<Noticias[]> {
    return new Promise(resolve => {
      this.http.get(`${ urlApi }/noticias`)
      .subscribe(res => {
        if (res['ok'] === true) {
          resolve(res['noticias']);
        } else {
          resolve(undefined);
        }
      });
    });    
  }
  async buscarNoticia(id: number): Promise<Noticias> {
    return new Promise(resolve => {
      this.http.get(`${ urlApi }/noticias/${id}`)
      .subscribe(res => {
        if (res['ok'] === true) {
          resolve(res['noticia']);
        } else {
          resolve(undefined);
        }
      });
    });
  }

  


  //validar token 
  async validateToken(redirect?: boolean): Promise<boolean> {
    const token = await this.storage.get('token') || undefined;
    if (!token) {
      if (redirect) {
        this.router.navigateByUrl('/login');
      }
      return Promise.resolve(false);
    }
    return new Promise<boolean>(resolve => {
      const headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      this.http.get(`${urlApi}/cuenta/userByToken`, { headers })
        .pipe(
          catchError(err => of({ ok: false }))
        )
        .subscribe(res => {
          if (res['ok'] === true) {
            this.usuario = res['usuario'];
            resolve(true);
          } else {
            resolve(false);
          }
        });
    });
  }




}
