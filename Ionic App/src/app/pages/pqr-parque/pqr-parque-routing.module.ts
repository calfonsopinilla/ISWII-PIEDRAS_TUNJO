import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PqrParquePage } from './pqr-parque.page';

const routes: Routes = [
  {
    path: '',
    component: PqrParquePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PqrParquePageRoutingModule {}
