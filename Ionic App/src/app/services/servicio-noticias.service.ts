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


  obtenerInformacionNoticias(): Observable<Noticias[]> {
    return this.http.get<Noticias[]>(`${urlApi}/comentariosNoticia/noticias`)
  }

  obtenerNoticiaVer(id): Observable<Noticias> {
    return this.http.get<Noticias>(`${urlApi}/comentariosNoticia/buscarNoticia1?id=${id}`)
  }

  async agregarComentario(comentario: any) {
    const token = await this.storage.get('token');
    if (!token) {
      this.router.navigateByUrl('/login');
    }
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();
    return new Promise(resolve => {
      const headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      this.http.post(`${urlApi}/comentariosNoticia`, comentario, { headers }).pipe(
        catchError(err => of({ ok: false }))
      )
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        }, err => { }, () => loading.dismiss());
    });
  }

  async eliminarComentario(id:any){
    const token = await this.storage.get('token');
    if (!token) {
      this.router.navigateByUrl('/login');
    }
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();
    return new Promise<boolean>(resolve => {
      const headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      console.log('Este es el token')
      console.log(headers);
      this.http.delete(`${urlApi}/comentariosNoticia/${id}`,{headers}).pipe(
        catchError(err => of({ ok: false }))
      )
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        }, err => { }, () => loading.dismiss());
    });
  }

  async reportarComentario(id:any){
    const token = await this.storage.get('token');
    if (!token) {
      this.router.navigateByUrl('/login');
    }
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();
    
    return new Promise<boolean>(resolve => {
      const headers = new HttpHeaders({
        Authorization: 'Bearer ' + token
      });
      console.log('Este es el token')
      console.log(headers);
      //`${urlApi}/comentariosNoticia/${id}`,{headers}
      this.http.post(`${urlApi}/comentariosNoticia/reportar`,id,{headers}).pipe(
        catchError(err => of({ ok: false }))
      )
        .subscribe(res => {
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        }, err => { }, () => loading.dismiss());
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
