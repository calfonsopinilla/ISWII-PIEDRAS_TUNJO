import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DetallesCabanaPageRoutingModule } from './detalles-cabana-routing.module';

import { DetallesCabanaPage } from './detalles-cabana.page';
import { ComponentsModule } from '../../../components/components.module';
import { PipesModule } from '../../../pipes/pipes.module';
import { IonicRatingModule } from 'ionic4-rating';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetallesCabanaPageRoutingModule,
    ComponentsModule,
    PipesModule,
    ReactiveFormsModule,
    IonicRatingModule
  ],
  declarations: [DetallesCabanaPage]
})
export class DetallesCabanaPageModule {}
