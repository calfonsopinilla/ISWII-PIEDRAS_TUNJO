import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'imgUrlToArray'
})
export class ImgUrlToArrayPipe implements PipeTransform {

  transform(imagenesUrl: string): string[] {
    const array = imagenesUrl.split('@');
    return array.filter(x => x !== '');
  }

}
