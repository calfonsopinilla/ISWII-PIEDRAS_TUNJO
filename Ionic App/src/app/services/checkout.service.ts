import { Injectable } from '@angular/core';
import { CustomerInfo } from '../interfaces/customerInfo.interface';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  payment(customerInfo: CustomerInfo) {
    return new Promise(async (resolve) => {
      const user = await this.authService.getUsuario();
      customerInfo.email = user.CorreoElectronico;
      this.http.post(`${ apiUrl }/stripe/payment?userId=${ user.Id }`, customerInfo)
                .subscribe(res => {
                  console.log(res);
                });
    });
  }
}
