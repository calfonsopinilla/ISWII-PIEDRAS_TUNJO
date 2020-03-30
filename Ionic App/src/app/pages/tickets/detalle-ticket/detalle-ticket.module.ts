import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DetalleTicketPageRoutingModule } from './detalle-ticket-routing.module';

import { DetalleTicketPage } from './detalle-ticket.page';
import { ComponentsModule } from '../../../components/components.module';
import { NgxQRCodeModule } from 'ngx-qrcode2';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DetalleTicketPageRoutingModule,
    ComponentsModule,
    NgxQRCodeModule
  ],
  declarations: [DetalleTicketPage]
})
export class DetalleTicketPageModule {}
