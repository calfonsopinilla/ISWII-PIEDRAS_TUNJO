import { Component, OnInit } from '@angular/core';
import { ItemInfo } from 'src/app/interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  rutas = ['/descripcion-parque', '/resenia-historica', '/ubicacion-parque', '/piedras-parque'];
  itemsInfo: ItemInfo[] = [];

  constructor(
    private infoParqueService: InfoParqueService
  ) { }

  ngOnInit() {
    this.infoParqueService.obtenerInfoParque()
                            .subscribe(resp => {
                              // console.log(resp);
                              this.itemsInfo = resp;
                            });
  }

}
