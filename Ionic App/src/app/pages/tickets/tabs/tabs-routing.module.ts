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
        path: 'compra',
        loadChildren: () => import('../compra/compra.module').then(mod => mod.CompraPageModule)
      },
      {
        path: '', pathMatch: 'full', redirectTo: 'inicio'
      },
      {
        path: '**', pathMatch: 'full', redirectTo: '/inicio'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
