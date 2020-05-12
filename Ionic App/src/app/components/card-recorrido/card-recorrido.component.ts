import { Component, OnInit, Input, ViewChild, AfterViewInit } from '@angular/core';
import * as Mapboxgl from 'mapbox-gl';
import { Recorrido } from '../../interfaces/recorrido.interface';
import { GeometryService } from '../../services/geometry.service';
import { environment } from 'src/environments/environment';
import { PuntoInteres } from '../../interfaces/punto-interes.interface';
import { Router } from '@angular/router';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-card-recorrido',
  templateUrl: './card-recorrido.component.html',
  styleUrls: ['./card-recorrido.component.scss'],
})

export class CardRecorridoComponent implements OnInit, AfterViewInit {

  @Input() recorrido: Recorrido;
  @ViewChild('map', { static: false }) map: any;
  mapbox: Mapboxgl.Map;

  geojson = {
    type: 'FeatureCollection',
    features: [
      {
        type: 'Feature',
        geometry: {
          type: 'LineString',
          coordinates: [
            // [-74.34453014946088, 4.814101441626363],
            // [-74.34201960182293, 4.817950186143946],
          ]
        }
      }
    ]
  };

  constructor(
    private geometryService: GeometryService,
    private alertCtrl: AlertController,
    private router: Router
  ) { }

  ngOnInit() {}

  ngAfterViewInit() {
    Mapboxgl.accessToken = environment.mapboxToken;
    this.mapbox = new Mapboxgl.Map({
      container: this.map.nativeElement,
      style: 'mapbox://styles/mapbox/streets-v11',
      center: [-74.3451602, 4.8154681],
      zoom: 14.5
    });

    this.mapbox.on('load', () => {
      // resize
      this.mapbox.resize();
      this.mapbox.addSource('line', {
        type: 'geojson',
        data: this.geojson
      });
      // add the line
      this.mapbox.addLayer({
        id: 'line-animation',
        type: 'line',
        source: 'line',
        layout: {
          'line-cap': 'round',
          'line-join': 'round'
        },
        paint: {
          'line-color': '#ed6498',
          'line-width': 5,
          'line-opacity': 0.8
        }
      });
      this.cargarRuta();
      this.cargarPtsControl();
      this.cargarPtsInteres();
    });

    // controls del mapa
    this.mapbox.addControl(new Mapboxgl.NavigationControl());
    // this.mapbox.addControl(new Mapboxgl.FullscreenControl());
  }

  cargarRuta() {
    const coordinates = this.geometryService.getCoordinates(this.recorrido.RutaText);
    this.geojson.features[0].geometry.coordinates = coordinates;
    this.mapbox.getSource('line').setData(this.geojson);
  }

  cargarPtsControl() {
    const ptsControl = JSON.parse(this.recorrido.PuntosControl);
    // console.log(ptsControl);
    ptsControl.forEach(x => {
      const popup = new Mapboxgl.Popup({offset: 25}).setText(x.nombre);
      new Mapboxgl.Marker({ color: '#d33' })
                  .setPopup(popup)
                  .setLngLat([x.longitud, x.latitud])
                  .addTo(this.mapbox);
    });
  }

  cargarPtsInteres() {
    const ptsInteres = JSON.parse(this.recorrido.PuntosInteres);
    // console.log(ptsInteres);
    ptsInteres.forEach((x: PuntoInteres) => {
      const popup = new Mapboxgl.Popup({offset: 25}).setText(x.Descripcion);
      new Mapboxgl.Marker()
                  .setPopup(popup)
                  .setLngLat([x.Longitud, x.Latitud])
                  .addTo(this.mapbox);
    });
  }

  detallesRecorrido(id: number) {
    this.router.navigate(['/detalles-recorrido', id]);
  }

}
