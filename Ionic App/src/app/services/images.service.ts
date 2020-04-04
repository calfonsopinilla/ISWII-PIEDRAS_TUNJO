import { Injectable } from '@angular/core';
import { FileTransfer, FileUploadOptions, FileTransferObject } from '@ionic-native/file-transfer/ngx';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(
    private fileTransfer: FileTransfer,
  ) { }

  uploadImage(imgData: string, tipo: string) {
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
    return fileTransfer
                    .upload(imgData, `${environment.servicesAPI}/images/uploadImage?tipo=${ tipo }`, options);
  }
}
