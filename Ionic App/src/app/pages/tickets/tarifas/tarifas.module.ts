import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TarifasPageRoutingModule } from './tarifas-routing.module';

import { TarifasPage } from './tarifas.page';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TarifasPageRoutingModule,
    ComponentsModule
  ],
  declarations: [TarifasPage]
})
export class TarifasPageModule {}
