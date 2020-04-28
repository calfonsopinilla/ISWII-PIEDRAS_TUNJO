import { Component, OnInit } from '@angular/core';
import { RecorridosService } from '../../services/recorridos.service';
import { Recorrido } from '../../interfaces/recorrido.interface';

@Component({
  selector: 'app-recorridos',
  templateUrl: './recorridos.page.html',
  styleUrls: ['./recorridos.page.scss'],
})
export class RecorridosPage implements OnInit {

  recorridos: Recorrido[] = [];

  constructor(
    private recorridosService: RecorridosService
  ) { }

  ngOnInit() {
    this.cargarRecorridos();
  }

  async cargarRecorridos() {
    this.recorridos = await this.recorridosService.getRecorridos();
    // console.log(this.recorridos);
  }

}
