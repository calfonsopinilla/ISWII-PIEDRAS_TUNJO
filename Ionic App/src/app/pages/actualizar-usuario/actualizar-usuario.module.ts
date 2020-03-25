import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ActualizarUsuarioPageRoutingModule } from './actualizar-usuario-routing.module';

import { ActualizarUsuarioPage } from './actualizar-usuario.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ActualizarUsuarioPageRoutingModule,
    ComponentsModule
  ],
  declarations: [ActualizarUsuarioPage]
})
export class ActualizarUsuarioPageModule {}
