import { Component, OnInit } from '@angular/core';
import { Noticias} from '../../interfaces/noticias';
import {ServicioNoticiasService }  from '../../services/servicio-noticias.service';
import {AuthService}  from '../../services/auth.service';
import { NavController, LoadingController,NavParams} from '@ionic/angular';
import { async } from '@angular/core/testing';
import {Router,NavigationExtras} from '@angular/router';
import { Pipe, PipeTransform } from '@angular/core';
import { NgForm } from '@angular/forms';
import { parse } from 'url';


@Component({
  selector: 'app-noticias',
  templateUrl: './noticias.page.html',
  styleUrls: ['./noticias.page.scss'],
})
export class NoticiasPage implements OnInit {

  constructor(public navCtrl: NavController ,
    public noticiaServicio : ServicioNoticiasService,
    public authServicio : AuthService,
    private loadingCtrl: LoadingController,
    private router: Router
   ) { }
   noticias : Noticias [] = [];
   noticiaSelect : Noticias ;
   session : boolean ; 
   fechaparse : Date; 
   registerUser = {comentar: ''};
   idNoticia  : string ;

   async ngOnInit() {
      this.session = await this.authServicio.isAuthenticated();
      this.noticiaServicio.obtenerInformacionNoticias().subscribe(resp => {this. noticias = resp
      });
    }
    // enviar datos 
    async verNoticia(id){
      console.log(this.noticias);
      const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
      await loading.present();
      this.noticiaSelect = this.noticias[id];

      let navigationPromocion : NavigationExtras = {
        queryParams : {
          special :  JSON.stringify(this.noticiaSelect)
        } 
      }
      this.router.navigate(['ver-noticia'],navigationPromocion);
      await loading.dismiss();
    }

    public agregarComentario(form: NgForm ){
      console.log(form.valid);
      console.log(form.value);
    }

}
