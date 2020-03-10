import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DescripcionParquePage } from './descripcion-parque.page';

const routes: Routes = [
  {
    path: '',
    component: DescripcionParquePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DescripcionParquePageRoutingModule {}
