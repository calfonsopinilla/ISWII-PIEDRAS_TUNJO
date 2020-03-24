import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegister } from '../interfaces/user-regster.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
}) export class UserService {

    constructor(
        private http: HttpClient,
        private storage: Storage
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
}