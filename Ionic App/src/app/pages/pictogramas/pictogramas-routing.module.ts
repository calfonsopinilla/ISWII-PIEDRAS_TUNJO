import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PictogramasPage } from './pictogramas.page';

const routes: Routes = [
  {
    path: '',
    component: PictogramasPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PictogramasPageRoutingModule {}
