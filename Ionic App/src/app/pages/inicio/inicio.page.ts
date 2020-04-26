import { Component, OnInit } from '@angular/core';
import { ItemInfo } from 'src/app/interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';
import { AuthService } from '../../services/auth.service';
import {FCM} from '@ionic-native/fcm/ngx';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  rutas = ['/descripcion-parque', '/resenia-historica', '/ubicacion-parque', '/piedras-parque'];
  itemsInfo: ItemInfo[] = [];
  horarioInfo: any;
  tokens: string ;

  constructor(
    private infoParqueService: InfoParqueService,
    private authService: AuthService,
    private router: Router,
    private fcm: FCM
  ) { }

  ngOnInit() {

    this.infoParqueService.obtenerInfoParque()
                            .subscribe(resp => {
                              this.itemsInfo = resp;
                              this.horarioInfo = (resp as ItemInfo[])
                                                  .find(x => x.id === 7); // find horario item
                            });
    // firebase
    this.fcm.getToken().then(token => {
        console.log(token);
        this.tokens = token;
     });

  }

}
