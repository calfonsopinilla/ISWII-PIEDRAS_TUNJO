import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TusReservasPage } from './tus-reservas.page';

const routes: Routes = [
  {
    path: '',
    component: TusReservasPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TusReservasPageRoutingModule {}
