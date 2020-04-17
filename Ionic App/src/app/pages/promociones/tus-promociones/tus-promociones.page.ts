import { Component, OnInit } from '@angular/core';
import { PromocionesService } from '../../../services/promociones.service';
import { ReservaPromocion } from '../../../interfaces/reserva-promocion.interface';

@Component({
  selector: 'app-tus-promociones',
  templateUrl: './tus-promociones.page.html',
  styleUrls: ['./tus-promociones.page.scss'],
})
export class TusPromocionesPage implements OnInit {

  reservasPromo: ReservaPromocion[] = [];

  constructor(
    private promoService: PromocionesService
  ) { }

  ngOnInit() {
    this.promoService.nuevaReservaPromo$.subscribe(_ => this.obtenerPromocionesUser());
    this.obtenerPromocionesUser();
  }

  async obtenerPromocionesUser() {
    this.reservasPromo = await this.promoService.getReservasByUserId();
  }

}
