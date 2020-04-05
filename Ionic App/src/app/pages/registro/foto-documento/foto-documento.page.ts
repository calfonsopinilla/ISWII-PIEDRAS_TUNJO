import { Component, OnInit } from '@angular/core';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { ImagesService } from '../../../services/images.service';
import { LoadingController, ToastController } from '@ionic/angular';

declare var window: any;

@Component({
  selector: 'app-foto-documento',
  templateUrl: './foto-documento.page.html',
  styleUrls: ['./foto-documento.page.scss'],
})
export class FotoDocumentoPage implements OnInit {

  imgData: any;
  img: any;
  response: any;

  constructor(
    private camera: Camera,
    private imagesService: ImagesService,
    private loadingCtrl: LoadingController,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
  }

  openCamera() {
    const options: CameraOptions = {
      quality: 100,
      destinationType: this.camera.DestinationType.FILE_URI,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      sourceType: this.camera.PictureSourceType.CAMERA,
      correctOrientation: false,
      saveToPhotoAlbum: false,
      allowEdit: false
    };

    this.camera.getPicture(options).then((imgData) => {
      this.imgData = imgData;
      this.img = window.Ionic.WebView.convertFileSrc(imgData);
    }).catch(err => {
      // Handle error
      console.log(err);
    });
  }

  async sendImage() {
    const loading = await this.loadingCtrl.create({ message: 'Subiendo imagen' });
    loading.present();
    this.imagesService.uploadImage(this.imgData, 'identificacion')
                      .then(res => {
                        this.response = res;
                        loading.dismiss();
                        this.presentToast(res.response);
                      });
  }

  async presentToast(message: any) {
    const toast = await this.toastCtrl.create({
      message: JSON.stringify(message),
      duration: 3000
    });
    await toast.present();
  }
}
