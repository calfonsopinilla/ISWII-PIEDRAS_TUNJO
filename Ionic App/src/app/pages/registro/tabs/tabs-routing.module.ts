import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TabsPage } from './tabs.page';

const routes: Routes = [
  {
    path: '',
    component: TabsPage,
    children: [
      {
        path: 'inicio',
        loadChildren: () => import('../inicio/registro.module').then(mod => mod.RegistroPageModule)
      },
      {
        path: 'ingresar-codigo',
        loadChildren: () => import('../ingresar-codigo/ingresar-codigo.module').then(mod => mod.IngresarCodigoPageModule)
      },
      {
        path: '**', pathMatch: 'full', redirectTo: 'inicio'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
