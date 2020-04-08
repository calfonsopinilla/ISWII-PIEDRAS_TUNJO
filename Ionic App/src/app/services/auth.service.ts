import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLogin } from '../interfaces/user-login.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { LoadingController, ToastController } from '@ionic/angular';
import { Router } from '@angular/router';
import { Usuario } from '../interfaces/usuario.interface';
import * as jwt_decode from 'jwt-decode';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  usuario: Usuario;
  loginState$ = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient,
    private storage: Storage,
    private loadingCtrl: LoadingController,
    private router: Router,
    private toastCtrl: ToastController
  ) { }

  async login(userLogin: UserLogin) {

    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();

    this.http.post(`${ urlApi }/cuenta/iniciaSesion`, userLogin)
              .pipe(catchError(err => {
                return of( err.error );
              }))
              .subscribe(
                async (res) => {
                  setTimeout(_ => {}, 1000); // timeout para el loadingCtrl
                  // Verificar la respuesta de la petición
                  if (res['ok'] === true) {
                    this.storage.clear();
                    // decode jwt
                    const decode = jwt_decode(res['token']);
                    const user = JSON.parse(decode['usuario']);
                    // no acceso al usuario administrador
                    if (user['RolId'] === 1) {
                      this.presentToast('No puedes acceder como administrador');
                      return;
                    }
                    // Almacenar token
                    this.guardarToken(res['token']);
                    // Redireccionamiento del usuario
                    this.loginNavigate(user);
                  } else {
                    this.presentToast(res['message']);
                  }
                },
                (err) => {},
                () => loading.dismiss()
              );
  }

  loginNavigate(userLogin: Usuario) {
    if (Number(userLogin['RolId'] === 2)) {
      if (!Boolean(userLogin['VerificacionCuenta']) && userLogin['LugarExpedicion'] == null) {
        this.router.navigate(['/registro', 'foto-documento']);
      } else {
        this.loginState$.emit(true);
        this.router.navigateByUrl('/inicio');
      }
    } else if (Number(userLogin['RolId'] === 4)) {
      this.router.navigateByUrl('/vigilante');
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 3000
    });
    await toast.present();
  }

  async isAuthenticated() {
    return await this.validateToken(false);
  }

  async getUsuario() {
    await this.validateToken(false);
    return {...this.usuario};
  }

  logout() {
    this.storage.clear(); // eliminamos el token
    this.loginState$.emit(false);
    this.router.navigate(['/inicio']);
  }

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

      this.http.get(`${ urlApi }/cuenta/userByToken`, { headers })
                .pipe(
                  catchError(err => of({ok: false}))
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

  async guardarToken(token: string) {
    await this.storage.set('token', token);
    await this.validateToken(false);
  }

  async actualizarUsuario(usuario: Usuario) {
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
      this.http.put(`${ urlApi }/cuenta/update/${ usuario.Id }`, usuario, { headers })
                .pipe(
                  catchError(err => of({ok: false}))
                )
                .subscribe(res => {
                  if (res['ok'] === true) {
                    this.storage.clear();
                    this.guardarToken(res['token']);
                    resolve(true);
                  } else {
                    resolve(false);
                  }
                }, err => {}, () => loading.dismiss());
    });
  }
}
