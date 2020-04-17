import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TusPromocionesPageRoutingModule } from './tus-promociones-routing.module';

import { TusPromocionesPage } from './tus-promociones.page';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TusPromocionesPageRoutingModule,
    ComponentsModule
  ],
  declarations: [TusPromocionesPage]
})
export class TusPromocionesPageModule {}
