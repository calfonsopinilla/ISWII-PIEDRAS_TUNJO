import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';

import { IonicModule } from '@ionic/angular';

import { DetalleEventoPageRoutingModule } from './detalle-evento-routing.module';

import { DetalleEventoPage } from './detalle-evento.page';

import { IonicRatingModule } from 'ionic4-rating';
import { PipesModule } from '../../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReactiveFormsModule,
    DetalleEventoPageRoutingModule,
    ComponentsModule,
    PipesModule,
    IonicRatingModule
  ],
  declarations: [DetalleEventoPage]
})
export class DetalleEventoPageModule {}
