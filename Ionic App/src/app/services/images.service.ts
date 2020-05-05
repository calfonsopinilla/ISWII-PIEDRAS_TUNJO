import { Injectable } from '@angular/core';
import { FileTransfer, FileUploadOptions, FileTransferObject } from '@ionic-native/file-transfer/ngx';
import { environment } from '../../environments/environment';
import { resolve } from 'url';
@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(
    private fileTransfer: FileTransfer
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
    return new Promise(resolve => {
      fileTransfer.upload(imgData, `${environment.servicesAPI}/images/dniImage?id=${ id }`, options)
                  .then(res => resolve({ ok: true, res }))
                  .catch(err => {
                    console.log('Error en carga', err);
                    resolve({ ok: false, err });
                  });
    });
  }

  async uploadImage(imgData: string, tipo: string): Promise<any> {
    const filename = imgData.split('/').pop();
    const fileTransfer: FileTransferObject = this.fileTransfer.create();

    const options: FileUploadOptions = {
      fileKey: 'image',
      fileName: filename,
      chunkedMode: false,
      mimeType: 'image/jpg',
      params: { title: 'title' }
    };

    return new Promise(resolve => {
      // Proceso de carga de imagen
      fileTransfer.upload(imgData, `${environment.servicesAPI}/images/uploadImage?tipo=${ tipo }`, options)
                  .then(res => resolve({ ok: true, res }))
                  .catch(err => {
                    console.log('Error en carga', err);
                    resolve({ ok: false, err });
                  });
    });
  }
}
