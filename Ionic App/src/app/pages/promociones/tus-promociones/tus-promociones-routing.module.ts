import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TusPromocionesPage } from './tus-promociones.page';

const routes: Routes = [
  {
    path: '',
    component: TusPromocionesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TusPromocionesPageRoutingModule {}
