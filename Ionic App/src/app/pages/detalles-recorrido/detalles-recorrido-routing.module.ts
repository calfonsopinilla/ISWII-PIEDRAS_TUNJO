import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DetallesRecorridoPage } from './detalles-recorrido.page';

const routes: Routes = [
  {
    path: '',
    component: DetallesRecorridoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DetallesRecorridoPageRoutingModule {}
