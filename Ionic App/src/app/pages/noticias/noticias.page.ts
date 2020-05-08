import { Component, OnInit,LOCALE_ID } from '@angular/core';
import { Noticias} from '../../interfaces/noticias';
import {ServicioNoticiasService }  from '../../services/servicio-noticias.service';
import {AuthService}  from '../../services/auth.service';
import { NavController, LoadingController,NavParams} from '@ionic/angular';
import {Router,NavigationExtras} from '@angular/router';

@Component({
  selector: 'app-noticias',
  templateUrl: './noticias.page.html',
  styleUrls: ['./noticias.page.scss'],
})
export class NoticiasPage implements OnInit {
  constructor(public navCtrl: NavController ,
    public noticiaServicio : ServicioNoticiasService,
    public authServicio : AuthService,
    private router: Router 
    ) { 
      this.ngOnInit();
    }
    noticias : Noticias [] = [];
    imagenes : string [] = [];
    session : boolean ; 
    async ngOnInit() {
      this.session = await this.authServicio.isAuthenticated();
      this.obtenerNoticias(); 
    }
    async obtenerNoticias(){
      this.noticias = await this.noticiaServicio.obtenerNoticias();
      for(let x = 0 ; x < this.noticias.length ; x++){
          this.imagenes=  this.getImages(this.noticias[x].imagenesUrl);
          this.noticias[x].listaImagenes = this.imagenes;
        }
    }
  //options mensaje no hay noticias
    slideOptions = {
      allowSlidePrev: false,
      allowSlideNext: false
    };
    getImages(imagenes :string) {
        const images = imagenes.split('@');
        return images;
    }
    verNoticia(id : number){
        this.router.navigate(['/ver-noticias', id]);
    }
}
