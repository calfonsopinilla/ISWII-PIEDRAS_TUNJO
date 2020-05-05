import { Injectable } from '@angular/core';
import { FileTransfer, FileUploadOptions, FileTransferObject } from '@ionic-native/file-transfer/ngx';
import { environment } from '../../environments/environment';
import { resolve } from 'url';
@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(
    private fileTransfer: FileTransfer,
  ) { }

  async uploadDni(imgData: string, id: number): Promise<any> {
    const filename = imgData.split('/').pop();
    const fileTransfer: FileTransferObject = this.fileTransfer.create();

    const options: FileUploadOptions = {
      fileKey: 'image',
      fileName: filename,
      chunkedMode: false,
      mimeType: 'image/jpg',
      params: { title: 'title' }
    };

    await fileTransfer
                    .upload(imgData, `${environment.servicesAPI}/images/dniImage?id=${ id }`, options)                                      
                    .then(res => {
                      return true;
                    }, err => {
                      return false;
                    });
  }

  async uploadImage(imgData: string, tipo: string) : Promise<any> {
    const filename = imgData.split('/').pop();
    const fileTransfer: FileTransferObject = this.fileTransfer.create();

    const options: FileUploadOptions = {
      fileKey: 'image',
      fileName: filename,
      chunkedMode: false,
      mimeType: 'image/jpg',
      params: { title: 'title' }
    };

    // Proceso de carga de imagen
    await fileTransfer
                    .upload(imgData, `${environment.servicesAPI}/images/uploadImage?tipo=${ tipo }`, options)                  
                    .then(res => {                      
                      if (res['ok'] === true) {
                        return true;
                      } else {
                        return false;
                      }                      
                    }, err => {
                      return false;
                    });
  }
}
