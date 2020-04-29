import { Component, OnInit, Input, ViewChild, AfterViewInit } from '@angular/core';
import { Pictograma } from '../../interfaces/pictograma.interface';
import * as Mapboxgl from 'mapbox-gl';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-card-pictograma',
  templateUrl: './card-pictograma.component.html',
  styleUrls: ['./card-pictograma.component.scss'],
})
export class CardPictogramaComponent implements OnInit, AfterViewInit {

  @Input() pictograma: Pictograma;
  @ViewChild('map', { static: false }) map: any;
  mapboxgl: Mapboxgl.Map;

  constructor() { }

  ngOnInit() {
    // console.log(this.pictograma);
  }

  ngAfterViewInit() {
    Mapboxgl.accessToken = environment.mapboxToken;
    this.mapboxgl = new Mapboxgl.Map({
      // container: 'map',
      container: this.map.nativeElement,
      style: 'mapbox://styles/mapbox/streets-v11',
      center: [this.pictograma.Longitud, this.pictograma.Latitud],
      zoom: 15
    });

    // Agregar Popup
    const popup = new Mapboxgl.Popup({ offset: 25 })
                              .setText(this.pictograma.Nombre);

    // Agregar el marcador del centro del parque
    new Mapboxgl.Marker({draggable: false})
                                .setLngLat([this.pictograma.Longitud, this.pictograma.Latitud])
                                .setPopup(popup)
                                .addTo(this.mapboxgl);
    // navigation controls
    this.mapboxgl.addControl(new Mapboxgl.NavigationControl());
  }

}
