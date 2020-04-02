import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdquirirPromocionPage } from './adquirir-promocion.page';

const routes: Routes = [
  {
    path: '',
    component: AdquirirPromocionPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdquirirPromocionPageRoutingModule {}
