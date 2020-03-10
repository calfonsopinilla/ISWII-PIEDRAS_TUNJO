import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'domSanitizer'
})
export class DomSanitizerPipe implements PipeTransform {

  constructor(private domSanitizer: DomSanitizer) {
  }

  transform(img: string): any {
    const domImage = `background-image: url('${img}')`;
    // confirmar que sea un estilo seguro
    return this.domSanitizer.bypassSecurityTrustStyle(domImage);
  }

}
