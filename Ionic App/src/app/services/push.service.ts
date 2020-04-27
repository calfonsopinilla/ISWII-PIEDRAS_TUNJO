import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AlertController, ToastController } from '@ionic/angular';
import { environment } from '../../environments/environment';

const apiUrl = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class PushService {

  private headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private toastCtrl: ToastController
  ) { }

  agregarToken(push : any){
    return this.http.post(`${ apiUrl }/push/crear`,push)                
                .subscribe(res => {
                    setTimeout(_ => {}, 2000);
                    this.presentToast(res['message']);
                },
                (err) => {
                  setTimeout(_ => {}, 2000);
                  this.presentToast(err);
                },
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
