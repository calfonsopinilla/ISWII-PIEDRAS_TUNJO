import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PictogramasPageRoutingModule } from './pictogramas-routing.module';

import { PictogramasPage } from './pictogramas.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PictogramasPageRoutingModule,
    ComponentsModule
  ],
  declarations: [PictogramasPage]
})
export class PictogramasPageModule {}
