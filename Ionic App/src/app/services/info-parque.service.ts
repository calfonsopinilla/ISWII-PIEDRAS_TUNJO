import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { ItemInfo } from '../interfaces/item-info.interface';
import { environment } from '../../environments/environment';

const servicesApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})

export class InfoParqueService {

  constructor(
    private http: HttpClient
  ) { }

  obtenerInfoParque(): Observable<ItemInfo[]> {
    return this.http.get<ItemInfo[]>(`${ servicesApi }/informacion/enviarInformacion`)
  }

  obtenerItemInfo(id: any): Observable<ItemInfo> {
    let item = null;
    this.obtenerInfoParque()
                  .subscribe((resp: ItemInfo[]) => {
                    item = resp.find(x => x.id === id);
                    return of(item);
                  });
    return of(item);
  }
}
