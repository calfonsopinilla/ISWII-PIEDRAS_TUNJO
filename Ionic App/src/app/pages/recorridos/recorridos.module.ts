import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { RecorridosPageRoutingModule } from './recorridos-routing.module';

import { RecorridosPage } from './recorridos.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RecorridosPageRoutingModule,
    ComponentsModule
  ],
  declarations: [RecorridosPage]
})
export class RecorridosPageModule {}
