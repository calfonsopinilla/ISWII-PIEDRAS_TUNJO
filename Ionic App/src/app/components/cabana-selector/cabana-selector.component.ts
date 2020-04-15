import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { CabanaService } from '../../services/cabana.service';
import { Cabana } from 'src/app/interfaces/cabana.interface';
import { IonSlides } from '@ionic/angular';

@Component({
  selector: 'app-cabana-selector',
  templateUrl: './cabana-selector.component.html',
  styleUrls: ['./cabana-selector.component.scss'],
})
export class CabanaSelectorComponent implements OnInit {

  @ViewChild('slides,', { static: false }) slides: IonSlides;

  @Output() cabanaSelected = new EventEmitter<{ id: number, valor: number }>();

  cabanas: Cabana[] = [];
  idSelected: number;
  // options slide
  cabanaSelector = {
    slidesPerView: 1.5,
    speed: 400
  };

  constructor(
    private cabanaService: CabanaService
  ) { }

  ngOnInit() {
    this.cargarCabanas();
  }

  async cargarCabanas() {
    this.cabanas = await this.cabanaService.getCabanas();
    this.idSelected = this.cabanas[0].Id;
    this.cabanaSelected.emit({ id: this.idSelected, valor: this.cabanas[0].Precio });
  }

  // getPrimerImagen(imagenesUrl: string) {
  //   return imagenesUrl.split('@')[0];
  // }

  seleccionarCabana(cabana: Cabana, index: number) {
    this.slides.slideTo(index, 400);
    this.idSelected = cabana.Id;
    this.cabanaSelected.emit({ id: cabana.Id, valor: cabana.Precio });
  }

}
