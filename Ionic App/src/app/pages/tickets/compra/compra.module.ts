import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CompraPageRoutingModule } from './compra-routing.module';
import { CompraPage } from './compra.page';
import { ComponentsModule } from '../../../components/components.module';
import { CheckoutPage } from '../../checkout/checkout.page';
import { CheckoutPageModule } from '../../checkout/checkout.module';
@NgModule({
  entryComponents: [CheckoutPage],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    CompraPageRoutingModule,
    ComponentsModule,
    CheckoutPageModule
  ],
  declarations: [CompraPage]
})
export class CompraPageModule {}
