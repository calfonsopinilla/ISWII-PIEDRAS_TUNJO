import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {

  itemsParque = [
    {
      label: 'Inicio',
      ruta: '/inicio',
      icon: 'home-outline'
    },
    {
      label: 'Descripción',
      ruta: '/descripcion-parque',
      icon: 'book-outline'
    },
    {
      label: 'Reseña histórica',
      ruta: '/resenia-historica',
      icon: 'book-outline'
    },
    {
      label: 'Ubicación',
      ruta: '/ubicacion-parque',
      icon: 'location-outline'
    },
    {
      label: 'Preguntas frecuentes (PQR)',
      ruta: '/pqr-parque',
      icon: 'help-circle-outline'
    }
  ];

  itemsServicios = [
    {
      label: 'Tickets',
      ruta: '/tickets',
      icon: 'map-outline'
    },
    {
      label: 'Reserva de Cabañas',
      ruta: '/reserva-cabanias',
      icon: 'calendar-outline'
    },
    {
      label: 'Reserva de Canoas',
      ruta: '/reserva-cabañas',
      icon: 'boat-outline'
    },
  ];

  itemsCuenta = [
    {
      label: 'Iniciar sesión',
      ruta: '/login',
      icon: 'log-in-outline'
    },
    {
      label: 'Registrarse',
      ruta: '/registro',
      icon: 'disc-outline'
    },
  ];

  constructor() { }

  ngOnInit() {}

}
