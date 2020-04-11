import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ModalComentarioPageRoutingModule } from './modal-comentario-routing.module';

import { ModalComentarioPage } from './modal-comentario.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ModalComentarioPageRoutingModule
  ],
  declarations: [ModalComentarioPage]
})
export class ModalComentarioPageModule {}
