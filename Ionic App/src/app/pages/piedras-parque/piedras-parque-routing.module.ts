import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PiedrasParquePage } from './piedras-parque.page';

const routes: Routes = [
  {
    path: '',
    component: PiedrasParquePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PiedrasParquePageRoutingModule {}
