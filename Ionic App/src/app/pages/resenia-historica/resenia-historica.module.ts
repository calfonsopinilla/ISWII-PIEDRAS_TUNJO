import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReseniaHistoricaPageRoutingModule } from './resenia-historica-routing.module';

import { ReseniaHistoricaPage } from './resenia-historica.page';
import { PipesModule } from '../../pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReseniaHistoricaPageRoutingModule,
    PipesModule
  ],
  declarations: [ReseniaHistoricaPage]
})
export class ReseniaHistoricaPageModule {}
