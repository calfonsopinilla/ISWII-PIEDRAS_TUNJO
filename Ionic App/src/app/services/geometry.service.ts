import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GeometryService {

  constructor() { }

  getCoordinates(lineString: string) {
    const coordinates = [];
    lineString = lineString.split('(')[1].replace(')', '');
    // console.log(lineString);
    lineString.split(',').forEach(x => {
      const [lng, lat] = x.split(' ');
      coordinates.push([Number(lng), Number(lat)]);
    });
    return coordinates;
  }
}
