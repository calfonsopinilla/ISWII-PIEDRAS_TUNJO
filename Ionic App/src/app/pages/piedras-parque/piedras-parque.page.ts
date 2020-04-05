import { Component, OnInit } from '@angular/core';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { InfoParqueService } from '../../services/info-parque.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-piedras-parque',
  templateUrl: './piedras-parque.page.html',
  styleUrls: ['./piedras-parque.page.scss'],
})
export class PiedrasParquePage implements OnInit {

  itemInfo: ItemInfo;  

  constructor(
    private infoParqueService: InfoParqueService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.infoParqueService.obtenerItemInfo(id)
                        .subscribe((resp: any) => this.itemInfo = resp);                        
  }

}
