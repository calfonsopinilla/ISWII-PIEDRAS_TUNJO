import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { environment } from '../../../environments/environment';

declare var mapboxgl: any;
const LONGITUD = -74.3460000;
const LATITUD = 4.8164000;

@Component({
  selector: 'app-item-info',
  templateUrl: './item-info.component.html',
  styleUrls: ['./item-info.component.scss'],
})
export class ItemInfoComponent implements AfterViewInit, OnInit {

  @Input() itemInfo: ItemInfo;
  @Input() ruta: string;
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
        center: [LONGITUD, LATITUD],
        zoom: 15
      });

      // Agregar Popup
      const popup = new mapboxgl.Popup({ offset: 25 })
                                .setText('Piedras del Tunjo');

      // Agregar el marcador del centro del parque
      const marker = new mapboxgl.Marker({draggable: false})
                                  .setLngLat([LONGITUD, LATITUD])
                                  .setPopup(popup)
                                  .addTo(map);

    }
  }

  ngOnInit() {
    // console.log(this.itemInfo);
  }

  esItemUbicacion() {
    return this.itemInfo.property.includes('Ubicaci');
  }

  esItemPiedras() {
    return this.itemInfo.property.includes('Piedras');
  }

}
