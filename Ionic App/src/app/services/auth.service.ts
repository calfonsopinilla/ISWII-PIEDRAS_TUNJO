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
import { PushService } from './push.service';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private usuario: Usuario;
  loginState$ = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient,
    private storage: Storage,
    private loadingCtrl: LoadingController,
    private router: Router,
    private toastCtrl: ToastController,
    private pushService: PushService
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
                    // console.log(user);
                    // no acceso al usuario administrador o cajero
                    if (user['RolId'] === 1 || user['RolId'] === 3) {
                      this.presentToast('No puedes acceder como administrador o cajero');
                      return;
                    }
                    // no acceso a usuario desactivado o deshabilitado
                    if (user['EstadoCuenta'] === false) {
                      this.presentToast('Tu cuenta ha sido deshabilitada.');
                      return;
                    }
                    // Actualizar push object
                    this.updatePush(Number(user['Id']));
                    // Almacenar token
                    this.guardarToken(res['token']);
                    // Redireccionamiento del usuario
                    this.loginNavigate(user);
                  } else {
                    this.presentToast('Usuario y/o contraseña incorrectos');
                  }
                },
                (err) => {},
                () => loading.dismiss()
              );
  }

  async updatePush(userId: number) {
    const push = await this.storage.get('push');
    push.UserId = userId;
    const respuest = this.pushService.agregarToken(push);
    await this.storage.set('push', push);
  }

  async loginNavigate(userLogin: Usuario) {
    await this.validateToken();
    if (Number(userLogin['RolId'] === 2)) {
      this.loginState$.emit(true);
      if (!Boolean(userLogin['VerificacionCuenta']) && userLogin['LugarExpedicion'] == null) {
        this.router.navigate(['/foto-documento']);
      } else {
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
    return await this.validateToken();
  }

  async getUsuario() {
    await this.validateToken();
    return {...this.usuario};
  }

  async logout() {
    this.updatePush(0);
    this.storage.remove('token'); // eliminamos el token
    this.loginState$.emit(false);
    this.router.navigate(['/inicio']);
  }

  async validateToken(redirect: boolean = false): Promise<boolean> {
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
                    this.updatePush(0);
                    this.storage.remove('token');
                    this.router.navigateByUrl('/login');
                    resolve(false);
                  }
                });
    });
  }

  async guardarToken(token: string) {
    await this.storage.set('token', token);
    await this.validateToken();
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
                    this.loginState$.emit(true);
                    resolve(true);
                  } else {
                    resolve(false);
                  }
                }, err => {}, () => loading.dismiss());
    });
  }

  esVigilante(redirect: boolean = true): Promise<boolean> {
    return new Promise(async resolve => {
      const usuario = await this.getUsuario();
      if (usuario.RolId !== 3) {
        if (redirect) {
          this.router.navigateByUrl('/inicio');
        }
      }
      resolve(usuario.RolId === 3);
    });
  }

  verifiedAccount(): Promise<boolean> {
    return new Promise(async resolve => {
      const usuario = await this.getUsuario();
      if (usuario.VerificacionCuenta === false) {
        this.router.navigateByUrl('/foto-documento');
      }
      resolve(usuario.VerificacionCuenta);
    });
  }

  async recuperarClave(correoElectronico: string, numeroDocumento: string) : Promise<boolean> {
    return new Promise(resolve => {
      this.http.get(`${ urlApi }/cuenta/recuperar-clave/generar-codigo?correoElectronico=${ correoElectronico }&numeroDocumento=${ numeroDocumento }`)      
        .pipe(          
          catchError(err => of({ok: false}))
        )
        .subscribe(res => {
          console.log(res);
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        });
    });
  }

  async cambiarClave(codigoVerificacion: string, clave: string) : Promise<boolean> {
    return new Promise(resolve => {
      this.http.get(`${ urlApi }/cuenta/recuperar-clave/cambiar?codigoVerificacion=${ codigoVerificacion }&clave=${ clave }`)
        .subscribe(res => {
          console.log(res);
          if (res['ok'] === true) {
            resolve(true);
          } else {
            resolve(false);
          }
        }, err => {          
          resolve(false);
        });
    });
  }
}
