import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'imgUrlToArray'
})
export class ImgUrlToArrayPipe implements PipeTransform {

  transform(imagenesUrl: string): string[] {
    return imagenesUrl.split('@');
  }

}
