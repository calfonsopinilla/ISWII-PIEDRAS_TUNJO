import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CheckoutPage } from './checkout.page';
import { ExpireDateDirective } from '../../directives/expire-date.directive';
import { MaxLengthDirective } from '../../directives/max-length.directive';
import { DirectivesModule } from '../../directives/directives.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    DirectivesModule
  ],
  declarations: [CheckoutPage]
})
export class CheckoutPageModule {}