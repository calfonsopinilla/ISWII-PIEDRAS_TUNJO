import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ModalComentarioPage } from './modal-comentario.page';

const routes: Routes = [
  {
    path: '',
    component: ModalComentarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ModalComentarioPageRoutingModule {}
