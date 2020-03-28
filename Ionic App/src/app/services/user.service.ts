import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegister } from '../interfaces/user-regster.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';
import { Usuario } from '../interfaces/usuario.interface';
import { LoadingController } from '@ionic/angular';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
}) export class UserService {

    constructor(
        private http: HttpClient,
        private storage: Storage,
        private loadingCtrl: LoadingController
    ) { }

    crearUsuario(user: UserRegister) {
        return this.http.post(`${ urlApi }/usuario/registro/generar_token`, user)
                .pipe(catchError(err => {
                    return of( err.error );
                }));
    }

    validarNumeroDocumentoCorreoElectronico(user: UserRegister) {
        return this.http.get(`${ urlApi }/Registro/Val_EmailYCC?valCorreo=${user.correoElectronico}&valDocumento=${user.numeroDocumento}`)
                .pipe(catchError(err => {
                    return of( err.error );
                }));
    }

    actualizarDatos(usuario: Usuario) {
        return new Promise(async resolve => {
            const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
            await loading.present();
            this.http.put(`${ urlApi }/usuarios/${ usuario.Id }`, usuario)
                        .subscribe(async res => {
                            if (res['ok'] === true) {
                                await this.storage.clear();
                                await this.storage.set('usuario', usuario);
                            }
                            resolve(res['ok']);
                        },
                        (err) => {},
                        () => loading.dismiss()
                    );
        });
    }
}