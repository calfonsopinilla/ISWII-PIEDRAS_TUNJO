import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TabsPage } from './tabs.page';

const routes: Routes = [
  {
    path: '',
    component: TabsPage,
    children: [
      {
        path: 'validar-qr',
        loadChildren: () => import('../validar-qr/validar-qr.module').then(mod => mod.ValidarQrPageModule)
      },
      {
        path: 'validar-numero-documento',
        loadChildren: () => import('../validar-numero-documento/validar-numero-documento.module').then(mod => mod.ValidarNumeroDocumentoPageModule)
      },
      {
        path: '', pathMatch: 'full', redirectTo: 'validar-qr'
      },
      {
        path: '**', pathMatch: 'full', redirectTo: 'validar-qr'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TabsPageRoutingModule {}
