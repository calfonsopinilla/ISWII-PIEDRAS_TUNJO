import { Injectable } from '@angular/core';
import { UserLogin } from '../interfaces/user-login.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(
    private http: HttpClient
  ) { }

  login(userLogin: UserLogin) {
    // console.log(userLogin);
    return new Promise(resolve => {
      this.http.post(`${ urlApi }/usuario/iniciaSesion`, userLogin)
                .pipe(catchError(err => {
                  resolve(false);
                  return err;
                }))
                .subscribe(resp => {
                  console.log(resp);
                  resolve( resp['ok'] ); // true or false
                });
    });
  }
}
