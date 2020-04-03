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
 
const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  loginState$ = new EventEmitter<boolean>();  
  user: any;

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
                    const ok = await this.storage.set('usuario', res['userLogin']);
                    // console.log(ok);
                    if (ok) {                                                                    

                      if (!Boolean(res['userLogin']['VerificacionCuenta'])) {
                        
                        console.log("Foto Documento");
                        this.router.navigate(['/registro', 'foto-documento']);

                      } else {

                        console.log("Inicio");
                        this.loginState$.emit(true);
                        this.router.navigateByUrl('/inicio');
                      }                      
                    }
                  } else {
                    this.presentToast(res['message']);
                  }
                },
                (err) => {},
                () => loading.dismiss()
              );
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
