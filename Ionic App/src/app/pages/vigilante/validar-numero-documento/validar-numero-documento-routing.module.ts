import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ValidarNumeroDocumentoPage } from './validar-numero-documento.page';

const routes: Routes = [
  {
    path: '',
    component: ValidarNumeroDocumentoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ValidarNumeroDocumentoPageRoutingModule {}
