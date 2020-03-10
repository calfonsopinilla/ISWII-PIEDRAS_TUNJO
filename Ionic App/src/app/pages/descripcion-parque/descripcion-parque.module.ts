import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DescripcionParquePageRoutingModule } from './descripcion-parque-routing.module';

import { DescripcionParquePage } from './descripcion-parque.page';
import { PipesModule } from '../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DescripcionParquePageRoutingModule,
    PipesModule
  ],
  declarations: [DescripcionParquePage]
})
export class DescripcionParquePageModule {}
