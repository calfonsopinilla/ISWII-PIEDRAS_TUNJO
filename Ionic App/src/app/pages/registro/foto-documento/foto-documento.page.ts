import { Component, OnInit } from '@angular/core';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavController, NavParams, LoadingController } from '@ionic/angular';
import { WebView } from '@ionic-native/ionic-webview/ngx';

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
    private camera: Camera,    
    public loadingCtrl: LoadingController,
    private http: HttpClient,
    private webView: WebView
  ) { }

  ngOnInit() {
  }

  takePicture() {

    const options: CameraOptions = {
      quality: 100,
      destinationType: this.camera.DestinationType.FILE_URI, // Recibe la imagen en Base64
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

    const formData = new FormData();
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

		);
  } 

}
