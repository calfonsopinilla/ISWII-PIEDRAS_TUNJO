import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PreguntasFrecuentesPageRoutingModule } from './preguntas-frecuentes-routing.module';

import { PreguntasFrecuentesPage } from './preguntas-frecuentes.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PreguntasFrecuentesPageRoutingModule,
    ComponentsModule
  ],
  declarations: [PreguntasFrecuentesPage]
})
export class PreguntasFrecuentesPageModule {}
