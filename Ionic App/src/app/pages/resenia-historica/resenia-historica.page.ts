import { Component, OnInit } from '@angular/core';
import { InfoParqueService } from '../../services/info-parque.service';
import { ItemInfo } from '../../interfaces/item-info.interface';

@Component({
  selector: 'app-resenia-historica',
  templateUrl: './resenia-historica.page.html',
  styleUrls: ['./resenia-historica.page.scss'],
})
export class ReseniaHistoricaPage implements OnInit {

  itemInfo: ItemInfo;

  constructor(
    private infoParqueService: InfoParqueService
  ) { }

  ngOnInit() {
    this.infoParqueService.obtenerItemInfo(2)
                          .subscribe((resp: any) => this.itemInfo = resp);
  }

}
