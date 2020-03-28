import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CuentaPageRoutingModule } from './cuenta-routing.module';
import { CuentaPage } from './cuenta.page';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    CuentaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [CuentaPage]
})
export class CuentaPageModule {}
