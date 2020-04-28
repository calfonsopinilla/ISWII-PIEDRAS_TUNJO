import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DetallesRecorridoPageRoutingModule } from './detalles-recorrido-routing.module';

import { DetallesRecorridoPage } from './detalles-recorrido.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetallesRecorridoPageRoutingModule
  ],
  declarations: [DetallesRecorridoPage]
})
export class DetallesRecorridoPageModule {}
