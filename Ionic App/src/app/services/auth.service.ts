import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../interfaces/user-login.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { LoadingController, ToastController } from '@ionic/angular';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';
import { Usuario } from '../interfaces/usuario.interface';
import * as jwt_decode from 'jwt-decode';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  loginState$ = new EventEmitter<boolean>();
  user: any;
  jsonToken: any;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private storage: Storage,
    private loadingCtrl: LoadingController,
    private router: Router,
    private toastCtrl: ToastController,
    private userService: UserService
  ) { }

  async login(userLogin: UserLogin) {
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();
    this.http.post(`${ urlApi }/cuenta/iniciaSesion`, userLogin)
              .pipe(
                catchError(err => {
                return of( err.error );
              }))
              .subscribe(
                async (res) => {
                  setTimeout(_ => {}, 1000);
                  // Verificar la respuesta de la petición
                  if (res['ok'] === true) {
                    if (res.userLogin.RolId === 1) {
                      this.presentToast('No puedes ingresar como usuario administrador');
                      return;
                    }
                    this.storage.clear();
                    const ok = await this.storage.set('token', res['token']);
                    if (ok) {
                      const jwt = jwt_decode(res['token']);
                      const user = JSON.parse(jwt['usuario']);
                      await this.storage.set('usuario', user);
                      this.loginNavigate(user);
                    }
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
      position: 'top',
      duration: 3000
    });
    await toast.present();
  }

  isAuthenticated(): Promise<boolean> {
    return new Promise<boolean>(async (resolve) => {
      const user = await this.storage.get('usuario') || null;
      resolve( user !== null );
    });
  }

  async getUsuario() {
    const usuario = await this.storage.get('usuario') || null;
    return usuario;
  }

  logout() {
    this.storage.clear();
    this.loginState$.emit(false);
    this.router.navigate(['/inicio']);
  }
}
