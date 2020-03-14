import { Component, OnInit } from '@angular/core';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-descripcion-parque',
  templateUrl: './descripcion-parque.page.html',
  styleUrls: ['./descripcion-parque.page.scss'],
})
export class DescripcionParquePage implements OnInit {

  itemInfo: ItemInfo;

  constructor(
    private infoParqueService: InfoParqueService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // this.route.paramMap
    //           .subscribe(params => console.log(params));
    this.infoParqueService.obtenerItemInfo(2)
                          .subscribe((resp: any) => {
                            this.itemInfo = resp;
                          });
  }

}
