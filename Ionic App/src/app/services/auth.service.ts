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

  /*
  TENGA EN CUENTA

  Aqui en este comentario le voy a poner la función de decodificar el token, ya usted vera como acomoda
  el resto del codigo porque toca tener en cuenta unas cosas
  */

  async login(userLogin: UserLogin) {

    const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    await loading.present();

    this.http.post(`${ urlApi }/cuenta/iniciaSesion`, userLogin)
              .pipe(catchError(err => {
                return of( err.error );
              }))
              .subscribe(
                async (res) => {
                  setTimeout(_ => {}, 2000); // timeout para el loadingCtrl
                  // Verificar la respuesta de la petición
                  if (res['ok'] === true) {
                    this.storage.clear();
                    // Almacenar el usuario en el localStorage
                    const ok = await this.storage.set('usuario', res['token']); // Ya no se guardaría userLogin - Se guardaria el token porque el token lleva la información del usuario codificada
                    // console.log(ok);
                    if (ok) {                                                                    
                      
                      this.jsonToken = jwt_decode(res['token'].data);
                      var parse = JSON.parse(res['token'].data);
                      this.storage.set("json", parse);
                      this.presentToast("Response with JSON.parse() :" + parse);
                      console.log(parse);
                      
                    } else {
                      console.log("ERROR");
                    }
                  } else {                    
                    this.presentToast(res['message']);
                  }
                },
                (err) => {},
                () => loading.dismiss()
              );
  }

  /*async login(userLogin: UserLogin) {
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
                    const ok = await this.storage.set('usuario', res['userLogin']);
                    if (ok) {
                      this.loginNavigate(res['userLogin']);
                    }
                  } else {
                    this.presentToast(res['message']);
                  }
                },
                (err) => {},
                () => loading.dismiss()
              );
  }
  */

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
    this.storage.remove('usuario');
    this.loginState$.emit(false);
    this.router.navigate(['/inicio']);
  }
}
