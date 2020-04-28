import { Component } from '@angular/core';
import { Platform } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';
import {Push, PushObject,PushOptions} from '@ionic-native/push/ngx';
import {PushService} from  '../app/services/push.service';


@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent {
  
  constructor(
    
    private platform: Platform,
    private splashScreen: SplashScreen,
    private statusBar: StatusBar,
    private push : Push,
    private pushService : PushService
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

  pushSetup(){

    const options: PushOptions = {
      android: {
          senderID: '253556781690'
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
   
   pushObject.on('notification').subscribe((notification: any) => console.log('Received a notification', notification));
   



   pushObject.on('registration').subscribe((registration: any ) =>  console.log('Device registered', registration));
   
   // insertar id del token 
   pushObject.on('registration').subscribe((registration: any ) => this.insertarToken(registration.registrationId) );

   pushObject.on('error').subscribe(error => console.error('Error with Push plugin', error));


   

  }


  async insertarToken(token:string){
    const push =  {
        ObjetoPush : token,
        Fecha : new Date(2000,1,1),
        EstadoId : 1,
        TokenId : token,
    }
    const respuest = this.pushService.agregarToken(push);
    console.log(respuest);
}


}
