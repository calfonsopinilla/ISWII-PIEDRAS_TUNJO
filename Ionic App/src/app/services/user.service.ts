import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { LoadingController, ToastController } from '@ionic/angular';
import { map, catchError } from 'rxjs/operators';
import { OneSignalService } from './one-signal.service';

const urlApi = environment.servicesAPI;
const redirectUrl = 'http://piedras-tunjo.herokuapp.com/usuarios';

@Injectable({
  providedIn: 'root'
}) export class UserService {

    constructor(
        private http: HttpClient,
        private storage: Storage,
        private toastCtrl: ToastController,
        private loadingCtrl: LoadingController,
        private oneSignalService: OneSignalService
    ) { }

    leerUsuario(id: number) {
        return this.http.get(`${ urlApi }/Usuarios?id=${ id }`);
    }

    async crearUsuario(user: any) {
        const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
        await loading.present();
        // console.log(user);
        return this.http.post(`${ urlApi }/usuario/registro/generar_token`, user)
                .pipe(catchError(err => {
                    return of( err.error );
                }))
                .subscribe(res => {
                    setTimeout(_ => {}, 2000);
                    this.oneSignalService.sendNotification('Nuevo usuario registrado', redirectUrl);
                    this.presentToast(res['message']);
                },
                (err) => {},
                () => loading.dismiss()
            );
    }

    async validarCodigo(code: string) {

        const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
        await loading.present();

        return this.http.get(`${ urlApi }/usuario/registro/validar_token?token=${code}`)
                .pipe(catchError(err => {
                    return of( err.error );
                }))
                .subscribe(res => {
                    setTimeout(_ => {}, 2000);
                    this.presentToast(res['message']);
                },
                (err) => {},
                () => loading.dismiss()
                );
    }

    existeCorreo(correo: string) {
        return this.http.get(`${ urlApi }/registro/existeCorreo?correo=${ correo }`)
                            .pipe(map(res => res['existe']));
    }

    existeNumeroDocumento(numeroDoc: string) {
        return this.http.get(`${ urlApi }/registro/existeNumeroDoc?numeroDoc=${ numeroDoc }`)
                        .pipe(map(res => res['existe']));
    }

    // Validar si existe correo en la tabla Token Correo
    existeCorreoToken(correo: string) {
        return this.http.get(`${ urlApi }/usuario/registro/existeCorreo?correo=${ correo }`)
                        .pipe(map(res => res['existe']));
    }

    // Validar si existe numero documento en la tabla Token Correo
    existeNumeroDocumentoToken(numeroDoc: string) {
        return this.http.get(`${ urlApi }/usuario/registro/existeNumeroDoc?numeroDoc=${ numeroDoc }`)
                        .pipe(map(res => res['existe']));
    }

    async presentToast(message: string) {
        const toast = await this.toastCtrl.create({
            message,
            duration: 3000
        });
        toast.present();
    }
}
