import { Component, OnInit } from '@angular/core';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { WebView } from '@ionic-native/ionic-webview/ngx';
import { FileTransfer, FileTransferObject, FileUploadOptions } from '@ionic-native/file-transfer/ngx';
import { NavController, LoadingController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-foto-documento',
  templateUrl: './foto-documento.page.html',
  styleUrls: ['./foto-documento.page.scss'],
})
export class FotoDocumentoPage implements OnInit {

  image: string;  
  imageB: string;
  imageTitle: any;
  isImageSelected:boolean = false;

  constructor(
    private transfer: FileTransfer,
    private camera: Camera,    
    public loadingCtrl: LoadingController,
    private http: HttpClient,
    private webView: WebView,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {
  }

  takePicture() {

    const options: CameraOptions = {
      quality: 100,
      destinationType: this.camera.DestinationType.FILE_URI,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      sourceType: this.camera.PictureSourceType.CAMERA    
    };

    this.camera.getPicture(options)
      .then((imageData) => {
        this.image = imageData;
        this.imageB = this.webView.convertFileSrc(imageData);  
        this.isImageSelected = true;        
      }, (err) => {
        alert(err);
      });
  }  

  doImageUpload() {               

    let filename = "Jhonattan";
    const fileTransfer: FileTransferObject = this.transfer.create();
 
    let options: FileUploadOptions = {
      fileKey: "file",
      fileName: filename,
      chunkedMode: false,
      mimeType: "image/jpg",
      params: { 'title': this.imageTitle }
    };
 
    fileTransfer.upload(this.image, "http://piedrasdeltunjo.tk/images/uploadImage?tipo=identificacion",options).then((res)=>{
      this.presentToast(res['message']);
    },(err)=>{
      this.presentToast("Ha ocurrido un error con el servidor");
    });    

    /*const formData = new FormData();
    formData.append('Jhonattan',this.image,'Jhonattan');
    return this.http.post("http://piedrasdeltunjo.tk/images/uploadImage?tipo=identificacion", formData)
    .subscribe(

			response => {				
				if(response <= 1){
					alert("Error en el servidor"); 
				}else{

					alert("Imagen enviada correctamente")

				}
			},
			error => {
				alert(<any>error);
			}

		);*/
  } 

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
      duration: 3000
    });
    await toast.present();
  }
}
