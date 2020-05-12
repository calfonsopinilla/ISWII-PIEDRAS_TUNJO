import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DetallePictogramaPage } from './detalle-pictograma.page';

const routes: Routes = [
  {
    path: '',
    component: DetallePictogramaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DetallePictogramaPageRoutingModule {}
