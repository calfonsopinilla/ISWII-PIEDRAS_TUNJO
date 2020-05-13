import { Injectable } from '@angular/core';
import { CustomerInfo } from '../interfaces/customerInfo.interface';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { LoadingController } from '@ionic/angular';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private loadingCtrl: LoadingController
  ) { }

  async payment(customerInfo: CustomerInfo) {
    const loading = await this.loadingCtrl.create({ message: 'Realizando pago...' });
    await loading.present();
    return new Promise(async (resolve) => {
      const user = await this.authService.getUsuario();
      customerInfo.email = user.CorreoElectronico;
      this.http.post(`${ apiUrl }/stripe/payment?userId=${ user.Id }`, customerInfo)
                .pipe
                (
                  catchError(err => {
                    console.log(err['error']['message']);
                    return of({ok: false, message: err['error']['message']});
                    }) // Internal Server Error
                )
                .subscribe(res => {
                  resolve(res);
                }, err => {}, () => loading.dismiss());
    });
  }
}
