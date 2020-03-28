import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReseniaHistoricaPage } from './resenia-historica.page';

const routes: Routes = [
  {
    path: '',
    component: ReseniaHistoricaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReseniaHistoricaPageRoutingModule {}
