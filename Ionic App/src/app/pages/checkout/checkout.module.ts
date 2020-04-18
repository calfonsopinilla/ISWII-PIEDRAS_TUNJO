import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CheckoutPage } from './checkout.page';
import { ExpireDateDirective } from '../../directives/expire-date.directive';
import { MaxLengthDirective } from '../../directives/max-length.directive';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule
  ],
  declarations: [CheckoutPage, MaxLengthDirective, ExpireDateDirective]
})
export class CheckoutPageModule {}
