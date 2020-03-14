import { Component, OnInit } from '@angular/core';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';

@Component({
  selector: 'app-piedras-parque',
  templateUrl: './piedras-parque.page.html',
  styleUrls: ['./piedras-parque.page.scss'],
})
export class PiedrasParquePage implements OnInit {

  itemInfo: ItemInfo;

  constructor(
    private infoParqueService: InfoParqueService
  ) { }

  ngOnInit() {
    this.infoParqueService.obtenerItemInfo(6)
                        .subscribe((resp: any) => this.itemInfo = resp);
  }

}
