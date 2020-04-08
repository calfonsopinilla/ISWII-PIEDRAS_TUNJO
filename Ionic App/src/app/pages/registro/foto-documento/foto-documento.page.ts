import { Component, OnInit } from '@angular/core';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { ImagesService } from '../../../services/images.service';
import { LoadingController, ToastController } from '@ionic/angular';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { Usuario } from '../../../interfaces/usuario.interface';

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

  constructor(
    private camera: Camera,
    private imagesService: ImagesService,
    private loadingCtrl: LoadingController,
    private toastCtrl: ToastController,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit() {

    this.authService.getUsuario().then(data => {
      this.user = data;      
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
    await this.imagesService.uploadImage(this.imgData, 'identificacion')
                      .then(res => {
                        this.response = res;

                        if (res['ok']) {
                          
                          this.userAux.Id = this.user['Id'];
                          this.userAux.Nombre = this.user['Nombre'];
                          this.userAux.Apellido = this.user['Apellido'];
                          this.userAux.CorreoElectronico = this.user['CorreoElectronico'];
                          this.userAux.Clave = this.user['Clave'];
                          this.userAux.NumeroDocumento = this.user['NumeroDocumento'];
                          this.userAux.TipoDocumento = this.user['TipoDocumento'];
                          this.userAux.LugarExpedicion = this.user['LugarExpedicion'];
                          this.userAux.Icono_url = this.user['Icono_url'];
                          this.userAux.VerificacionCuenta = this.user['VerificacionCuenta'];
                          this.userAux.EstadoCuenta = this.user['EstadoCuenta'];
                          this.userAux.RolId = this.user['RolId'];
                          this.userAux.Imagen_documento = this.user['Imagen_documento'];
                          this.userAux.Token = this.user['Token'];

                          this.userService.actualizarDatos(this.userAux);
                        }

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
