import { Component, OnInit } from '@angular/core';
import { PromocionesService } from '../../../services/promociones.service';
import { Promocion } from '../../../interfaces/promocion.interface';
import { ReservaPromocion } from '../../../interfaces/reserva-promocion.interface';
import { Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { CheckoutPage } from '../../checkout/checkout.page';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  promociones: Promocion[] = [];

  constructor(
    private promocionesService: PromocionesService,
    private router: Router,
    private modalCtrl: ModalController
  ) { }

  ngOnInit() {
    this.obtenerPromociones();
  }

  async obtenerPromociones() {
    this.promociones = await this.promocionesService.getPromociones();
  }

  async comprar(promo: Promocion) {
    // Checkout para el pago de la promoción
    const modal = await this.modalCtrl.create({
      component: CheckoutPage,
      componentProps: {
        amount: promo.Precio
      }
    });
    await modal.present();

    const { data } = await modal.onDidDismiss();
    console.log(data);

    if (data) {
      if (data.ok === true) {
        const reservaPromo: ReservaPromocion = {
          FechaCompra: new Date(),
          Precio: promo.Precio,
          UPromocionId: promo.Id
        };
        const done = await this.promocionesService.reservarPromocion(reservaPromo);
        if (done) {
          this.router.navigateByUrl('/promociones/tus-promociones');
        }
      }
    }

  }

}
