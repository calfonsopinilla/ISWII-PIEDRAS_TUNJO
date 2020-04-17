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
        loadChildren: () => import('../inicio/inicio.module').then(mod => mod.InicioPageModule)
      },
      {
        path: 'tus-promociones',
        loadChildren: () => import('../tus-promociones/tus-promociones.module').then(mod => mod.TusPromocionesPageModule)
      },
      { path: '', pathMatch: 'full', redirectTo: 'inicio' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
