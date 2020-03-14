import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { InfoParqueService } from '../../services/info-parque.service';
import { ItemInfo } from '../../interfaces/item-info.interface';
import { environment } from '../../../environments/environment';

declare var mapboxgl: any;
const LONGITUD = -74.3460000;
const LATITUD = 4.8164000;

@Component({
  selector: 'app-ubicacion-parque',
  templateUrl: './ubicacion-parque.page.html',
  styleUrls: ['./ubicacion-parque.page.scss'],
})
export class UbicacionParquePage implements OnInit, AfterViewInit {

  @ViewChild('map', { static: false }) map: any;
  itemInfo: ItemInfo;

  constructor(
    private infoParqueService: InfoParqueService
  ) { }

  ngAfterViewInit() {
    mapboxgl.accessToken = environment.mapboxToken;
    const map = new mapboxgl.Map({
      // container: 'map',
      container: this.map.nativeElement,
      style: 'mapbox://styles/mapbox/streets-v11',
      center: [LONGITUD, LATITUD],
      zoom: 15
    });

    map.on('load', () => {

      map.resize();

      // geojson
      map.addSource('route', {
        type: 'geojson',
        data: {
          type: 'Feature',
          properties: {},
          geometry: {
            type: 'LineString',
            coordinates: [
              [-74.34453014946088, 4.814101441626363],
              [-74.34201960182293, 4.817950186143946],
              [-74.34334997749423, 4.819062041614913],
              [-74.34453014946088, 4.8191261870676385],
              [-74.34641842460722, 4.818506114106768],
              [-74.34701923942664, 4.817886040580234],
              [-74.3495297870646, 4.816218253806653],
              [-74.34910063362206, 4.815106393685042],
              [-74.3484139881138, 4.814379407237453],
              [-74.34626822090179, 4.81333169010945],
              [-74.34671883201656, 4.811749420367704],
              [-74.34521679496835, 4.811471453682273],
              [-74.34453014946088, 4.814101441626363]
            ]
          }
        }
      });

      // layer
      map.addLayer({
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

    });

    map.on('click', (e: any) => {
      const { lng, lat } = e.lngLat;
      // console.log(`${lng}, ${lat}`);
    });

    const popup = new mapboxgl.Popup({offset: 25}).setText('Piedras del Tunjo');
    const marker = new mapboxgl.Marker({draggable: false})
                                      .setLngLat([LONGITUD, LATITUD])
                                      .setPopup(popup)
                                      .addTo(map);
  }

  ngOnInit() {
    this.infoParqueService.obtenerItemInfo(5)
                          .subscribe((resp: any) => this.itemInfo = resp);
  }

}
