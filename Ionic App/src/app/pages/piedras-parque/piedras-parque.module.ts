import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PiedrasParquePageRoutingModule } from './piedras-parque-routing.module';

import { PiedrasParquePage } from './piedras-parque.page';
import { PipesModule } from '../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PiedrasParquePageRoutingModule,
    PipesModule
  ],
  declarations: [PiedrasParquePage]
})
export class PiedrasParquePageModule {}
