import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DetalleTicketPageRoutingModule } from './detalle-ticket-routing.module';

import { DetalleTicketPage } from './detalle-ticket.page';
import { ComponentsModule } from '../../../components/components.module';
import { NgxQRCodeModule } from 'ngx-qrcode2';

import { TransferirTicketPage } from '../../transferir-ticket/transferir-ticket.page';
import { TransferirTicketPageModule } from '../../transferir-ticket/transferir-ticket.module';

@NgModule({
  entryComponents: [TransferirTicketPage],
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetalleTicketPageRoutingModule,
    ComponentsModule,
    NgxQRCodeModule,    
    TransferirTicketPageModule
  ],
  declarations: [DetalleTicketPage]
})
export class DetalleTicketPageModule {}
