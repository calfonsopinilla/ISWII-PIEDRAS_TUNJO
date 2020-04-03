import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizerPipe } from './dom-sanitizer.pipe';
import { ImgServerPipe } from './img-server.pipe';
import { TransformFechaPipe } from './transform-fecha.pipe';

@NgModule({
  declarations: [
    DomSanitizerPipe,
    ImgServerPipe,
    TransformFechaPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    DomSanitizerPipe,
    ImgServerPipe
  ]
})
export class PipesModule { }
