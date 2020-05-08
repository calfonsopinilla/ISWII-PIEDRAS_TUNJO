import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VerNoticiasPage } from './ver-noticias.page';

const routes: Routes = [
  {
    path: '',
    component: VerNoticiasPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VerNoticiasPageRoutingModule {}
