import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { environment } from '../../../environments/environment';
import * as Mapboxgl from 'mapbox-gl';

// declare var mapboxgl: any;
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
  mapa: Mapboxgl.Map;
  @ViewChild('map', { static: false }) map: any;

  slidesOptions = {
  };

  constructor() { }

  ngAfterViewInit() {
    if ( this.esItemUbicacion() ) {
      Mapboxgl.accessToken = environment.mapboxToken;
      this.mapa = new Mapboxgl.Map({
        // container: 'map',
        container: this.map.nativeElement,
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [LONGITUD, LATITUD],
        zoom: 15
      });

      this.mapa.on('load', () => {
        this.mapa.resize();
      });

      // Agregar Popup
      const popup = new Mapboxgl.Popup({ offset: 25 })
                                .setText('Piedras del Tunjo');

      // Agregar el marcador del centro del parque
      new Mapboxgl.Marker({draggable: false})
                                  .setLngLat([LONGITUD, LATITUD])
                                  .setPopup(popup)
                                  .addTo(this.mapa);
      // navigation controls
      this.mapa.addControl(new Mapboxgl.NavigationControl());
    }
  }

  ngOnInit() {
    // console.log(this.itemInfo);
  }

  esItemUbicacion() {
    return this.itemInfo.property.includes('Ubicaci');
  }

  esItemPiedras() {
    return this.itemInfo.property.includes('Folklore');
  }

}
