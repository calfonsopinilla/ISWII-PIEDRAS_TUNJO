import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { AdquirirPromocionPageRoutingModule } from './adquirir-promocion-routing.module';
import { AdquirirPromocionPage } from './adquirir-promocion.page';
import { PipesModule } from '../../pipes/pipes.module';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AdquirirPromocionPageRoutingModule,
    PipesModule,
    ComponentsModule
  ],
  declarations: [AdquirirPromocionPage]
})
export class AdquirirPromocionPageModule {}
