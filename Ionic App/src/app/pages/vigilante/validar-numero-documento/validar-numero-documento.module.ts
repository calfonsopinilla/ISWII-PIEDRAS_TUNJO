import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ValidarNumeroDocumentoPageRoutingModule } from './validar-numero-documento-routing.module';

import { ValidarNumeroDocumentoPage } from './validar-numero-documento.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ValidarNumeroDocumentoPageRoutingModule
  ],
  declarations: [ValidarNumeroDocumentoPage]
})
export class ValidarNumeroDocumentoPageModule {}
