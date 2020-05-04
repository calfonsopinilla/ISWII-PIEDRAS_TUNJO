import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpireDateDirective } from './expire-date.directive';
import { MaxLengthDirective } from './max-length.directive';
// import { EmailDirective } from './email.directive';
// import { NumeroDocDirective } from './numero-doc.directive';

@NgModule({
  declarations: [
    // EmailDirective,
    // NumeroDocDirective,
    ExpireDateDirective,
    MaxLengthDirective,
  ],
  imports: [
    CommonModule
  ],
  exports: [
    // EmailDirective,
    // NumeroDocDirective,
    ExpireDateDirective,
    MaxLengthDirective,
  ],
})
export class DirectivesModule { }
