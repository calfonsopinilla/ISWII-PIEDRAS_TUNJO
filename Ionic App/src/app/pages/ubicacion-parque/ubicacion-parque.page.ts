import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { InfoParqueService } from '../../services/info-parque.service';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { environment } from '../../../environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { CoordenadasService } from '../../services/coordenadas.service';
import * as Mapboxgl from 'mapbox-gl';
import { PuntoInteres } from 'src/app/interfaces/punto-interes.interface';
import { PuntosInteresService } from '../../services/puntos-interes.service';
import { Geolocation } from '@ionic-native/geolocation/ngx';
import { AlertController } from '@ionic/angular';

// declare var mapboxgl: any;
const LONGITUD = -74.3460000;
const LATITUD = 4.8164000;

@Component({
  selector: 'app-ubicacion-parque',
  templateUrl: './ubicacion-parque.page.html',
  styleUrls: ['./ubicacion-parque.page.scss'],
})
export class UbicacionParquePage implements OnInit, AfterViewInit {

  @ViewChild('map', { static: false }) map: any;
  mapa: Mapboxgl.Map;
  itemInfo: ItemInfo;
  flyDone = false;

  constructor(
    private infoParqueService: InfoParqueService,
    private route: ActivatedRoute,
    private coordenadasService: CoordenadasService,
    private puntosIntService: PuntosInteresService,
    private geolocation: Geolocation,
    private alertCtrl: AlertController,
    private router: Router
  ) { }

  ngAfterViewInit() {
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

      // geojson
      this.mapa.addSource('route', {
        type: 'geojson',
        data: {
          type: 'Feature',
          properties: {},
          geometry: {
            type: 'LineString',
            coordinates: this.coordenadasService.getLimites()
          }
        }
      });

      // layer
      this.mapa.addLayer({
        id: 'route',
        type: 'line',
        source: 'route',
        layout: {
          'line-join': 'round',
          'line-cap': 'round'
        },
        paint: {
          'line-color': '#888',
          'line-width': 4
        }
      });

      // Cargar markers
      this.puntosIntService.getPuntosInteres()
                            .subscribe(res => {
                              this.loadPuntosInteres(res);
                            });
    });

    this.mapa.on('click', (e: any) => {
      const { lng, lat } = e.lngLat;
    });

    // controls del mapa
    this.mapa.addControl(new Mapboxgl.NavigationControl());
  }

  loadPuntosInteres(puntosInt: PuntoInteres[]) {
    // console.log(puntosInt);
    puntosInt.forEach(punto => {
      const popup = new Mapboxgl.Popup({offset: 25})
                                .setText(punto.Descripcion);
      new Mapboxgl.Marker({draggable: false})
                                        .setLngLat([punto.Longitud, punto.Latitud])
                                        .setPopup(popup)
                                        .addTo(this.mapa);
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.infoParqueService.obtenerItemInfo(id)
                          .subscribe((resp: any) => this.itemInfo = resp);
  }

  obtenerGeolocation() {
    this.geolocation.getCurrentPosition().then((resp) => {
      const { latitude, longitude } = resp.coords;
      // console.log(resp.coords);
      this.flyLocation(longitude, latitude);
     }).catch(async (error) => {
        console.log('Error getting location', error);
        const alert = await this.alertCtrl.create({
          header: 'No se pudo obtener tu localización',
          message: 'Por favor, active la localización del dispositivo y si es necesario reinicie la aplicación',
          buttons: ['OK']
        });
        await alert.present();
     });
  }

  flyLocation(lng: number, lat: number) {
    // para no repetir markers
    if (!this.flyDone) {
      const popup = new Mapboxgl.Popup({offset: 25}).setText('You');
      new Mapboxgl.Marker({draggable: false, color: '#000'})
                  .setLngLat([lng, lat])
                  .setPopup(popup)
                  .addTo(this.mapa);
      this.flyDone = true;
    }
    // fly to location
    this.mapa.flyTo({
      center: [lng, lat],
      essential: true // this animation is considered essential with respect to prefers-reduced-motion
    });
  }

}
