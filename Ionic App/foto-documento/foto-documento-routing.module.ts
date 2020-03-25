import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FotoDocumentoPage } from './foto-documento.page';

const routes: Routes = [
  {
    path: '',
    component: FotoDocumentoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FotoDocumentoPageRoutingModule {}
