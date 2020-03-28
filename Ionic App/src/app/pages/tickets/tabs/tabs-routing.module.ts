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
        path: 'detalle-ticket/:id',
        loadChildren: () => import('../detalle-ticket/detalle-ticket.module').then( m => m.DetalleTicketPageModule)
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
