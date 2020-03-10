import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { UbicacionParquePageRoutingModule } from './ubicacion-parque-routing.module';

import { UbicacionParquePage } from './ubicacion-parque.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    UbicacionParquePageRoutingModule
  ],
  declarations: [UbicacionParquePage]
})
export class UbicacionParquePageModule {}
