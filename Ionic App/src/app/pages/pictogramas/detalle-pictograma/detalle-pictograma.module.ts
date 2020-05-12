import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';

import { IonicModule } from '@ionic/angular';

import { DetallePictogramaPageRoutingModule } from './detalle-pictograma-routing.module';

import { DetallePictogramaPage } from './detalle-pictograma.page';

import { IonicRatingModule } from 'ionic4-rating';
import { PipesModule } from '../../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetallePictogramaPageRoutingModule,
    ReactiveFormsModule,
    PipesModule,
    ComponentsModule,
    IonicRatingModule
  ],
  declarations: [DetallePictogramaPage]
})
export class DetallePictogramaPageModule {}
