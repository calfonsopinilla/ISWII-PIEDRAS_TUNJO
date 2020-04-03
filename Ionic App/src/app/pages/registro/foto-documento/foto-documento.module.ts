import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { FotoDocumentoPageRoutingModule } from './foto-documento-routing.module';

import { FotoDocumentoPage } from './foto-documento.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FotoDocumentoPageRoutingModule
  ],
  declarations: [FotoDocumentoPage]
})
export class FotoDocumentoPageModule {}
