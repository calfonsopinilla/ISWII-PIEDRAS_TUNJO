import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { LoadingController, ToastController } from '@ionic/angular';

const apiUrl = environment.servicesAPI;
import { of, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class PushService {
  
  constructor( private http: HttpClient,
              private toastCtrl :ToastController
    ) { }

  agregarToken(push : any){
    return this.http.post(`${ apiUrl }/push/crear`,push)
                .pipe(catchError(err => {
                    return of( err.error );
                }))
                .subscribe(res => {
                    setTimeout(_ => {}, 2000);
                    this.presentToast(res['message']);
                },
                (err) => {},
            );
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
        message,
        duration: 3000
    });
    toast.present();
  }
}
