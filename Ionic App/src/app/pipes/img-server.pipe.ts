import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../environments/environment';

@Pipe({
  name: 'imgServer'
})
export class ImgServerPipe implements PipeTransform {

  transform(img: string, service: string): any {
    const url = `${ environment.servicesAPI }/${ service }/?nombre=${img}`;
    return url;
  }

}
