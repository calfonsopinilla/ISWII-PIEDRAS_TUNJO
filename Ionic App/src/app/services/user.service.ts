import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegister } from '../interfaces/user-regster.interface';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
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

    public crearUsuario(user: UserRegister) {
        return this.http.post(`${ urlApi }/usuario/registro/generar_token`, user)
                .pipe(catchError(err => {
                    return of( err.error );
                }));
    }

<<<<<<< HEAD
    public validarNumeroDocumentoCorreoElectronico(user: UserRegister) {        
=======
    validarNumeroDocumentoCorreoElectronico(user: UserRegister) {
>>>>>>> 4ee2411ee5db71dbb7c9a4b9ca0fc00b3b533a8d
        return this.http.get(`${ urlApi }/Registro/Val_EmailYCC?valCorreo=${user.correoElectronico}&valDocumento=${user.numeroDocumento}`)
                .pipe(catchError(err => {
                    return of( err.error );
                }));
    }

<<<<<<< HEAD
    public obtenerImagenesAvatar(nombre: number) {
        return this.http.get(`${ urlApi }/images/getImage?tipo=identificacion&nombre=${nombre}.png`)
                .pipe(catchError(err => {
                    return of( err.error );
                }));
=======
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
>>>>>>> 4ee2411ee5db71dbb7c9a4b9ca0fc00b3b533a8d
    }
}