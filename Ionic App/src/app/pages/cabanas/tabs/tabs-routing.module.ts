import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TabsPage } from './tabs.page';

const routes: Routes = [
  {
    path: '',
    component: TabsPage,
    children: [
      {
        path: 'reservar',
        loadChildren: () => import('../reservar/reservar.module').then(mod => mod.ReservarPageModule)
      },
      {
        path: 'tus-reservas',
        loadChildren: () => import('../tus-reservas/tus-reservas.module').then(mod => mod.TusReservasPageModule)
      },
      { path: '', pathMatch: '', redirectTo: 'tus-reservas' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
