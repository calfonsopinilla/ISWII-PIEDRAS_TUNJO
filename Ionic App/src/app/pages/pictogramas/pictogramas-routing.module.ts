import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PictogramasPage } from './pictogramas.page';

const routes: Routes = [
  {
    path: '',
    component: PictogramasPage
  },  {
    path: 'detalle-pictograma',
    loadChildren: () => import('./detalle-pictograma/detalle-pictograma.module').then( m => m.DetallePictogramaPageModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PictogramasPageRoutingModule {}
