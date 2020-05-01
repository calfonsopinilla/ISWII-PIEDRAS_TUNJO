import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tarifas',
  templateUrl: './tarifas.page.html',
  styleUrls: ['./tarifas.page.scss'],
})
export class TarifasPage implements OnInit {

  slidesOpts = {
  };

  tarifas = [
    {
      text: 'Adultos',
      img: 'adultos.jpg',
      precio: 4800
    },
    {
      text: 'Nacidos en Facatativá',
      img: 'parque-faca.jpg',
      precio: 1800
    },
    {
      text: 'Niños de los 5 a los 10 años',
      img: 'ninos.jpg',
      precio: 1800
    },
    {
      text: 'Menores de 5 años y mayores de 65 años, excentos de pago',
      img: 'ancianos_ninos.jpg',
      precio: 0
    },
  ];

  constructor() { }

  ngOnInit() {
  }

}
