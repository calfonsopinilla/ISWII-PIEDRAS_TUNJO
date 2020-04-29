import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DetallesCabanaPage } from './detalles-cabana.page';

const routes: Routes = [
  {
    path: '',
    component: DetallesCabanaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DetallesCabanaPageRoutingModule {}
