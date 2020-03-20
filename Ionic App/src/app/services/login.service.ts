import { Injectable } from '@angular/core';
import { UserLogin } from '../interfaces/user-login.interface';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor() { }

  login(userLogin: UserLogin) {
    console.log(userLogin);
  }
}
