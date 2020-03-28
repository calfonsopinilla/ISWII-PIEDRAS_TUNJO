import { Component, OnInit } from '@angular/core';
import { Suscripciones } from '../../interfaces/suscripciones.interface';
import { SuscripcionService } from '../../services/suscripcion.service';

@Component({
  selector: 'app-suscripciones',
  templateUrl: './suscripciones.page.html',
  styleUrls: ['./suscripciones.page.scss'],
})
export class SuscripcionesPage implements OnInit {

  // Variables
  suscripciones: Suscripciones[] = [];

  constructor(
    private servicioSuscripcion: SuscripcionService
  ) { }

  ngOnInit() {
    this.servicioSuscripcion.leerSuscripciones().subscribe(resp => { this.suscripciones = resp });
  }

}
