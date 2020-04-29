import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecorridosService } from '../../services/recorridos.service';
import { Recorrido } from 'src/app/interfaces/recorrido.interface';
import * as Mapboxgl from 'mapbox-gl';
import { environment } from '../../../environments/environment';
import { GeometryService } from '../../services/geometry.service';
import { PuntoInteres } from '../../interfaces/punto-interes.interface';

const LONGITUD = -74.3459602;
const LATITUD = 4.8154681;

@Component({
  selector: 'app-detalles-recorrido',
  templateUrl: './detalles-recorrido.page.html',
  styleUrls: ['./detalles-recorrido.page.scss'],
})
export class DetallesRecorridoPage implements OnInit, AfterViewInit {

  @ViewChild('map', { static: false }) map: any;
  mapbox: Mapboxgl.Map;
  recorrido: Recorrido = {};

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
    private route: ActivatedRoute,
    private recorridosService: RecorridosService,
    private geometryService: GeometryService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.init();
  }

  async init() {
    // Obtener el recorrido
    const id = this.route.snapshot.paramMap.get('id');
    await this.obtenerRecorrido(Number(id));
    // Map
    Mapboxgl.accessToken = environment.mapboxToken;
    this.mapbox = new Mapboxgl.Map({
      container: this.map.nativeElement,
      style: 'mapbox://styles/mapbox/streets-v11',
      center: [LONGITUD, LATITUD],
      zoom: 15
    });

    this.mapbox.on('load', () => {
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
  }

  async obtenerRecorrido(id: number) {
    const recorridos = await this.recorridosService.getRecorridos();
    this.recorrido = recorridos.find(x => x.Id === id);
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

}
