import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

const urlApi = environment.servicesAPI;

@Injectable({
  providedIn: 'root'
})
export class ImagenService {

  constructor(
    private http: HttpClient
  ) { }

  uploadImage(formData: FormData) {
    return this.http.post(`${ urlApi }/images/uploadImage?tipo=identificacion`, formData);
  }
}
