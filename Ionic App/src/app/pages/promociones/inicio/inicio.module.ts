import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InicioPageRoutingModule } from './inicio-routing.module';

import { InicioPage } from './inicio.page';
import { ComponentsModule } from '../../../components/components.module';
import { CheckoutPage } from '../../checkout/checkout.page';
import { CheckoutPageModule } from '../../checkout/checkout.module';

@NgModule({
  entryComponents: [CheckoutPage],
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InicioPageRoutingModule,
    ComponentsModule,
    CheckoutPageModule
  ],
  declarations: [InicioPage]
})
export class InicioPageModule {}
