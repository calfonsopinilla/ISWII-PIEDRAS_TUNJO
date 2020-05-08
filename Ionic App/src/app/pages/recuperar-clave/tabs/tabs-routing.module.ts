import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TabsPage } from './tabs.page';

const routes: Routes = [
  {
    path: '',
    component: TabsPage,
    children: [
      {
        path: 'recuperar',
        loadChildren: () => import('../recuperar/recuperar.module').then(mod => mod.RecuperarPageModule)
      },
      {
        path: 'ingresar-codigo',
        loadChildren: () => import('../ingresar-codigo/ingresar-codigo.module').then(mod => mod.IngresarCodigoPageModule)
      },
      {
        path: '', pathMatch: 'full', redirectTo: 'recuperar'
      },
      {
        path: '**', pathMatch: 'full', redirectTo: 'recuperar'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
