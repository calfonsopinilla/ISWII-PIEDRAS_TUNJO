import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComponentsModule } from '../../components/components.module';

import { IonicModule } from '@ionic/angular';

import { TransferirTicketPageRoutingModule } from './transferir-ticket-routing.module';

import { TransferirTicketPage } from './transferir-ticket.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TransferirTicketPageRoutingModule,
    ReactiveFormsModule,
    ComponentsModule
  ],
  declarations: [TransferirTicketPage]
})
export class TransferirTicketPageModule {}
