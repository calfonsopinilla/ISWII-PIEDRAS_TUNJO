import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TransferirTicketPage } from './transferir-ticket.page';

const routes: Routes = [
  {
    path: '',
    component: TransferirTicketPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TransferirTicketPageRoutingModule {}
