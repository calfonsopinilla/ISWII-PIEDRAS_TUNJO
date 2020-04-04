import { Component, OnInit } from '@angular/core';
import { Router,ActivatedRoute, Route} from '@angular/router';
import {Noticias} from '../../interfaces/noticias';
import { async } from '@angular/core/testing';
import {AuthService}  from '../../services/auth.service';
import { CommonModule, JsonPipe } from '@angular/common';
import { NgForm } from '@angular/forms';
import { ServicioNoticiasService} from '../../services/servicio-noticias.service'; 
  



@Component({
  selector: 'app-ver-noticia',
  templateUrl: './ver-noticia.page.html',
  styleUrls: ['./ver-noticia.page.scss'],
})


export class VerNoticiaPage implements OnInit {

  data : any 
  idNoticia : string;
  noticia : Noticias ;
  session : boolean ;  
  registerUser = {comentar: ''};


  constructor(private activeRoute :ActivatedRoute ,private router: Router,public authServicio : AuthService,
   
    public noticiaServicio : ServicioNoticiasService, ) { 
      this.activeRoute.queryParams.subscribe(params => {
        console.log('params: ',params);
          if(params && params.special){
              this.data = params.special;
          }
      })
    
    }
    
   ngOnInit() {
    //this.noticiaServicio.obtenerNoticiaVer(15).subscribe(resp => {this.noticia1 = resp});    
     this.noticia =JSON.parse(this.data);
    console.log(this.noticia);
    }


  public agregarComentario(form : NgForm){
      console.log(form.valid);
      console.log(form.value);
  }


}
