import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UbicacionParquePage } from './ubicacion-parque.page';

const routes: Routes = [
  {
    path: '',
    component: UbicacionParquePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UbicacionParquePageRoutingModule {}
