import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TusReservasPageRoutingModule } from './tus-reservas-routing.module';

import { TusReservasPage } from './tus-reservas.page';
import { ComponentsModule } from '../../../components/components.module';
import { PipesModule } from '../../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TusReservasPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [TusReservasPage]
})
export class TusReservasPageModule {}
