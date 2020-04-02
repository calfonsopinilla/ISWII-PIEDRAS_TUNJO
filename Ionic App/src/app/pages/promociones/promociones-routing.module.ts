import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {} from './promociones.page' 


import { PromocionesPage } from './promociones.page';

const routes: Routes = [
  {
    path: '',
    component: PromocionesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PromocionesPageRoutingModule {}
