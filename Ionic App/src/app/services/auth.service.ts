import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../interfaces/user-login.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { Storage } from '@ionic/storage';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private storage: Storage
  ) { }

  login(userLogin: UserLogin) {
    return this.http.post(`${ urlApi }/usuario/iniciaSesion`, userLogin)
              .pipe(catchError(err => {
                return of( err.error );
              }));
  }

  isAuthenticated(): Promise<boolean> {
    return new Promise<boolean>(async (resolve) => {
      const user = await this.storage.get('usuario') || null;
      resolve( user !== null );
    });
  }

  logout() {
    this.storage.remove('usuario');
  }
}
