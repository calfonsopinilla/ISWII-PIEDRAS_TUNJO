import { Component, OnInit } from '@angular/core';
import { PictogramaService } from '../../services/pictograma.service';
import { Pictograma } from '../../interfaces/pictograma.interface';

@Component({
  selector: 'app-pictogramas',
  templateUrl: './pictogramas.page.html',
  styleUrls: ['./pictogramas.page.scss'],
})
export class PictogramasPage implements OnInit {

  pictogramas: Pictograma[] = [];

  constructor(
    private pictogramasService: PictogramaService
  ) { }

  ngOnInit() {
    this.cargarPictogramas();
  }

  async cargarPictogramas() {
    this.pictogramas = await this.pictogramasService.getPictogramas();
  }

}
