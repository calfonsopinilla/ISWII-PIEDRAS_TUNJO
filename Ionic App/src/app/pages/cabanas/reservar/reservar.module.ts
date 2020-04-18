import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { ReservarPageRoutingModule } from './reservar-routing.module';
import { ReservarPage } from './reservar.page';
import { ComponentsModule } from '../../../components/components.module';
import { CheckoutPage } from '../../checkout/checkout.page';
import { CheckoutPageModule } from '../../checkout/checkout.module';

@NgModule({
  entryComponents: [CheckoutPage],
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReservarPageRoutingModule,
    ComponentsModule,
    CheckoutPageModule
  ],
  declarations: [ReservarPage]
})
export class ReservarPageModule {}
