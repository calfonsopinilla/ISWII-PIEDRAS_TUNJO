import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { Usuario } from '../interfaces/usuario.interface';
import { LoadingController, ToastController } from '@ionic/angular';
import { map, catchError } from 'rxjs/operators';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
}) export class UserService {

    constructor(
        private http: HttpClient,
        private storage: Storage,
        private toastCtrl: ToastController,
        private loadingCtrl: LoadingController,
    ) { }

    leerUsuario(id: number) {
        return this.http.get(`${ urlApi }/Usuarios?id=${ id }`);
    }

    async crearUsuario(user: any) {
        const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
        await loading.present();
        console.log(user);

        return this.http.post(`${ urlApi }/usuario/registro/generar_token`, user)
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

    // actualizarDatos(usuario: Usuario) {
    //     return new Promise(async resolve => {
    //         const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
    //         await loading.present();
    //         this.http.put(`${ urlApi }/usuarios/${ usuario.Id }`, usuario)
    //                     .subscribe(async res => {
    //                         if (res['ok'] === true) {
    //                             await this.storage.clear();
    //                             await this.storage.set('usuario', usuario);
    //                         }
    //                         resolve(res['ok']);
    //                     },
    //                     (err) => {},
    //                     () => loading.dismiss()
    //                 );
    //     });
    // }

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
