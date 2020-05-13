import { Component } from '@angular/core';
import { Platform, AlertController } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';
import {Push, PushObject, PushOptions} from '@ionic-native/push/ngx';
import {PushService} from '../app/services/push.service';
import { Router} from '@angular/router';
import { AuthService } from './services/auth.service';
import { Storage } from '@ionic/storage';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent {

  constructor(
    private alertCtrl: AlertController,
    private router: Router,
    private platform: Platform,
    private splashScreen: SplashScreen,
    private statusBar: StatusBar,
    private push: Push,
    private pushService: PushService,
    private authService: AuthService,
    private storage: Storage
    ) {
    this.initializeApp();
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.statusBar.styleDefault();
      this.splashScreen.hide();
      this.pushSetup();
    });
  }

  async pushSetup() {
    console.log('PushSetup');
    const options: PushOptions = {
      android: {
        senderID: '253556781690',
        forceShow : true
      },
      ios: {
        alert: 'true',
        badge: true,
        sound: 'true'
      },
      windows: {},
      browser: {
        pushServiceURL: 'http://push.api.phonegap.com/v1/push'
      }
   };

    const pushObject: PushObject = this.push.init(options);
    pushObject.on('notification').subscribe((data: any) => {
      console.log('object notification -> ', data);
      const tipo = data.additionalData.tipo;
      console.log(data.additionalData.foreground, 'variable de open notification');
      if (data.additionalData.foreground) {
        console.log('Push notification clicked cerrada la aplicacion');
        this.action(tipo, data.message);
      } else {
        console.log('Push notification clicked abierta la aplicacion');
        this.action(tipo, data.message);
      }
    });

    pushObject.on('registration')
             .subscribe((registration: any ) =>  console.log('Device registered', registration));

    pushObject.on('registration')
              .subscribe((registration: any ) => this.insertarToken(registration.registrationId));
  }

  async insertarToken(token: string) {
    const push =  {
      ObjetoPush : token,
      Fecha : new Date(2000, 1, 1),
      EstadoId : 1,
      UserId : 0
    };
    await this.storage.set('push', push);
    const respuest = this.pushService.agregarToken(push);
  }

  async action(tipo: string , mensaje: string) {
    if (tipo === 'Noticia') {
      const confirmAlert =  await this.alertCtrl.create({
        message: mensaje,
        buttons: [{
          text: 'Cerrar',
          role: 'cancelar'
        }, {
          text: 'Ver noticias',
          handler: () => {
            this.router.navigateByUrl('/noticias');
          }
        }]
      });
      confirmAlert.present();

    } else if (tipo === 'Evento') {
      const confirmAlert = await this.alertCtrl.create({
        message: mensaje,
        buttons: [{
          text: 'Cerrar',
          role: 'cancelar'
        }, {
          text: 'Ver eventos',
          handler: () => {
            this.router.navigateByUrl('/eventos')
          }
        }]
      });
      confirmAlert.present();

    } else if (tipo === 'Promocion') {
      const confirmAlert =  await this.alertCtrl.create({
        message: mensaje,
        buttons: [{
          text: 'Cerrar',
          role: 'cancelar'
        }, {
          text: 'Ver promociones',
          handler: () => {
            this.router.navigateByUrl('/promociones');
          }
        }]
      });
      confirmAlert.present();
    } else if (tipo === 'Validado') {
      const confirmAlert =  await this.alertCtrl.create({
        message: mensaje,
        buttons: [{
          text: 'Iniciar de nuevo sesión',
          handler: () => {
            this.authService.logout();
          }
        }]
      });
      confirmAlert.present();
    }

  }

}
