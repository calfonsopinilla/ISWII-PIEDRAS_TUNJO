import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PqrParquePageRoutingModule } from './pqr-parque-routing.module';

import { PqrParquePage } from './pqr-parque.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PqrParquePageRoutingModule,
    ComponentsModule
  ],
  declarations: [PqrParquePage]
})
export class PqrParquePageModule {}
