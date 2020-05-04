import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizerPipe } from './dom-sanitizer.pipe';
import { ImgServerPipe } from './img-server.pipe';
import { TransformFechaPipe } from './transform-fecha.pipe';
import { ImgUrlToArrayPipe } from './img-url-to-array.pipe';

@NgModule({
  declarations: [
    DomSanitizerPipe,
    ImgServerPipe,
    TransformFechaPipe,
    ImgUrlToArrayPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    DomSanitizerPipe,
    ImgServerPipe,
    ImgUrlToArrayPipe
  ]
})
export class PipesModule { }
