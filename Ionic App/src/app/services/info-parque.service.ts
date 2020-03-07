import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { ItemInfo } from '../interfaces/item-info.interface';

@Injectable({
  providedIn: 'root'
})

export class InfoParqueService {

  itemsInfo: ItemInfo[] = [
    { id: 1,
      property: 'Descripción del parque',
      text: 'Este es el ejemplo de un texto de información',
      images: ['/assets/mock-images/default-image.jfif', '/assets/mock-images/default-image.jfif']
    },
    { id: 2,
      property: 'Reseña historica del parque',
      text: 'Este es el ejemplo de un texto de información',
      images: ['/assets/mock-images/default-image.jfif', '/assets/mock-images/default-image.jfif']
    },
    { id: 3,
      property: 'Ubicación del parque',
      text: 'Este es el ejemplo de un texto de información',
      images: ['/assets/mock-images/default-image.jfif', '/assets/mock-images/default-image.jfif']
    },
    { id: 4,
      property: 'Piedras del parque',
      text: 'Este es el ejemplo de un texto de información',
      images: ['/assets/mock-images/default-image.jfif', '/assets/mock-images/default-image.jfif']
    }
  ];

  constructor(
    private http: HttpClient
  ) { }

  obtenerInfoParque(): Observable<ItemInfo[]> {
    return of(this.itemsInfo);
  }
}
