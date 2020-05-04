import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Cabana } from '../../../interfaces/cabana.interface';
import { CabanaService } from '../../../services/cabana.service';

@Component({
  selector: 'app-detalles-cabana',
  templateUrl: './detalles-cabana.page.html',
  styleUrls: ['./detalles-cabana.page.scss'],
})
export class DetallesCabanaPage implements OnInit {

  cabana: Cabana = undefined;

  constructor(
    private route: ActivatedRoute,
    private cabanaService: CabanaService
  ) { }

  ngOnInit() {
    this.cargarCabana();
  }

  async cargarCabana() {
    const id = this.route.snapshot.paramMap.get('id');
    this.cabana = await this.cabanaService.getCabana(Number(id));
    // console.log(this.cabana);
  }

}
