import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { ItemInfo } from 'src/app/interfaces/item-info.interface';
import { environment } from '../../../environments/environment';

declare var mapboxgl: any;

@Component({
  selector: 'app-item-info',
  templateUrl: './item-info.component.html',
  styleUrls: ['./item-info.component.scss'],
})
export class ItemInfoComponent implements AfterViewInit {

  @Input() itemInfo: ItemInfo;
  @ViewChild('map', { static: false }) map: any;

  slidesOptions = {
  };

  constructor() { }

  ngAfterViewInit() {
    if ( this.esItemUbicacion() ) {
      mapboxgl.accessToken = environment.mapboxToken;
      const map = new mapboxgl.Map({
        // container: 'map',
        container: this.map.nativeElement,
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [-74.3450000, 4.8164000],
        zoom: 15
      });

      // Agregar Popup
      const popup = new mapboxgl.Popup({ offset: 25 })
                                .setText('Piedras del Tunjo');

      // Agregar el marcador del centro del parque
      const marker = new mapboxgl.Marker({draggable: false})
                                  .setLngLat([-74.3457000, 4.8164000])
                                  .setPopup(popup)
                                  .addTo(map);

    }
  }

  esItemUbicacion() {
    return this.itemInfo.property.includes('Ubicaci');
  }

  esItemPiedras() {
    return this.itemInfo.property.includes('Piedras');
  }

}
