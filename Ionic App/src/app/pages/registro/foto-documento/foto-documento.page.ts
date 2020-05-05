import { Component, OnInit } from '@angular/core';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { ImagesService } from '../../../services/images.service';
import { LoadingController, ToastController } from '@ionic/angular';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { Usuario } from '../../../interfaces/usuario.interface';
import { Router } from '@angular/router';

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
  user: any;
  userAux: Usuario;

  // verificar que la imagen esté pendiente de revisión
  pendingConfirmation = false;
  slidesOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  constructor(
    private router: Router,
    private camera: Camera,
    private imagesService: ImagesService,
    private loadingCtrl: LoadingController,
    private toastCtrl: ToastController,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.comprobar();
  }

  comprobar() {
    this.authService.getUsuario().then(data => {
      this.user = data;
      this.pendingConfirmation = this.user.Imagen_documento !== null;
    });
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
    const answer: boolean = await this.imagesService.uploadDni(this.imgData, this.user['Id']);
    loading.dismiss();
    this.user.Imagen_documento = this.imgData.split('/').pop();
    this.presentToast('Imagen subida con exito al servidor');
    this.router.navigate(['/inicio']);
  }

  async presentToast(message: any) {
    const toast = await this.toastCtrl.create({
      message: JSON.stringify(message),
      duration: 3000
    });
    await toast.present();
  }
}
