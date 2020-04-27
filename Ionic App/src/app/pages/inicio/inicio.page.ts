import { Component, OnInit } from '@angular/core';
import { ItemInfo } from 'src/app/interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';
import { AuthService } from '../../services/auth.service';
//import {FCM} from '@ionic-native/fcm/ngx';
//import { PushService } from '../../services/push.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  rutas = ['/descripcion-parque', '/resenia-historica', '/ubicacion-parque', '/piedras-parque'];
  itemsInfo: ItemInfo[] = [];
  horarioInfo: any;
  tokens : string ;
  constructor(
    private infoParqueService: InfoParqueService
    //private fcm : FCM,
    //private pushService: PushService
  ) { }

  ngOnInit() {
    this.infoParqueService.obtenerInfoParque()
                            .subscribe(resp => {
                              this.itemsInfo = resp;
                              this.horarioInfo = (resp as ItemInfo[])
                                                  .find(x => x.id === 7); // find horario item
                            });

    /*this.fcm.getToken().then(token => {      
      this.insertarToken(token);
      this.tokens = token;
    });*/
  }

  /*async insertarToken(token:string){
    const push =  {
        ObjetoPush : token,
        Fecha : new Date(2000,1,1),
        EstadoId : 1,
        TokenId : token,
    }
    alert("Token generado: " + token);
    const respuest = this.pushService.agregarToken(push);
    console.log(respuest);
    alert(respuest);
  }*/
}
