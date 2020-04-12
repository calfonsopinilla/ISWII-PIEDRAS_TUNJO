import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NoticiasPage } from './noticias.page';

const routes: Routes = [
  {
    path: '',
    component: NoticiasPage
  },
  {
    path: 'modal-comentario',
    loadChildren: () => import('./modal-comentario/modal-comentario.module').then( m => m.ModalComentarioPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NoticiasPageRoutingModule {}
