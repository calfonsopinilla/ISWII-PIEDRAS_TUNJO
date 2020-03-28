import { Component, OnInit } from '@angular/core';
import { InfoParqueService } from '../../services/info-parque.service';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-resenia-historica',
  templateUrl: './resenia-historica.page.html',
  styleUrls: ['./resenia-historica.page.scss'],
})
export class ReseniaHistoricaPage implements OnInit {

  itemInfo: ItemInfo;

  constructor(
    private infoParqueService: InfoParqueService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.infoParqueService.obtenerItemInfo(id)
                          .subscribe((resp: any) => {
                            this.itemInfo = resp;
                          });
  }

}
