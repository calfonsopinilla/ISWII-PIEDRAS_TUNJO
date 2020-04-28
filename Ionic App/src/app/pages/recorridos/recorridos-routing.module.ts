import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RecorridosPage } from './recorridos.page';

const routes: Routes = [
  {
    path: '',
    component: RecorridosPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RecorridosPageRoutingModule {}
