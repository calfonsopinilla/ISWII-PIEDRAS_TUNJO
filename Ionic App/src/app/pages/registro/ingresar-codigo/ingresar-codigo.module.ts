import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';

import { IonicModule } from '@ionic/angular';

import { IngresarCodigoPageRoutingModule } from './ingresar-codigo-routing.module';

import { IngresarCodigoPage } from './ingresar-codigo.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    IngresarCodigoPageRoutingModule,
    ComponentsModule
  ],
  declarations: [IngresarCodigoPage]
})
export class IngresarCodigoPageModule {}
