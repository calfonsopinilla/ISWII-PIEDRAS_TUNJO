import { Component, OnInit } from '@angular/core';
import { Router,ActivatedRoute, Route} from '@angular/router';
import {Noticias} from '../../interfaces/noticias';
import { async } from '@angular/core/testing';
import {AuthService}  from '../../services/auth.service';
import { NgForm } from '@angular/forms';
import { ServicioNoticiasService} from '../../services/servicio-noticias.service'; 
import { Usuario} from '../../interfaces/usuario.interface';
import { ModalController} from  '@ionic/angular'; 
import { ModalComentarioPage}  from '../../pages/noticias/modal-comentario/modal-comentario.page';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-ver-noticia',
  templateUrl: './ver-noticia.page.html',
  styleUrls: ['./ver-noticia.page.scss'],
})


export class VerNoticiaPage implements OnInit {

  data : any 
  idNoticia : string;
  noticia : Noticias ;
  session : boolean; 
  registerUser = {comentar: ''};
  usuario : Usuario;
  validar2 : boolean;
  
  constructor(private activeRoute :ActivatedRoute ,private router: Router,public authServicio : AuthService,
   
    public noticiaServicio : ServicioNoticiasService,
    private modalCtrlr : ModalController,
    private toastCntrl : ToastController ) { 
    }
    validar : boolean = true  ;
    id : any ;
    async ngOnInit() {
    if(this.validar == true ){
        //this.noticiaServicio.obtenerNoticiaVer(15).subscribe(resp => {this.noticia1 = resp});    
      this.activeRoute.queryParams.subscribe(params => {
        
          if(params && params.special){
              this.data = params.special;
          }
        })   
      this.noticia =JSON.parse(this.data);
    }else{
      this.id = this.noticia.id;
      this.noticiaServicio.obtenerNoticiaVer(this.id).subscribe(resp => {this.noticia = resp});    
    }
    this.sessions();
   }
    async sessions(){
      this.session = await this.authServicio.isAuthenticated();
      if(this.session == true ){
        this.authServicio.getUsuario().then(user => {
        this.usuario = user;})
        console.log(this.usuario);
        }
    }
    datas :any;
    async agregarComentario(form : NgForm){     
      
      if(form.valid==true){
        var comentario = {}
        comentario["usuarioId"]=this.usuario.Id;
        comentario["descripcion"]=this.registerUser.comentar;
        comentario["noticia_id"]= this.noticia.id;
        comentario["calificacion"] = 0;
        console.log(comentario);
        const resultado = await this.noticiaServicio.agregarComentario(comentario);    
        console.log(resultado);
        this.noticiaServicio.obtenerNoticiaVer(this.noticia.id).subscribe(resp => {this.noticia = resp});    
        this.registerUser.comentar="";
        this.validar=false;
        this.ngOnInit();
      }
  }

  async abrirOpciones(idComent,idUser,idUserComent){
    
    if(idUser != idUserComent){
      this.validar2 =  false
    }else{
      this.validar2 = true;
    }
    const modal = await this.modalCtrlr.create({ 
        component: ModalComentarioPage,
        cssClass: 'my-custom-modal-css',
        componentProps :{
          idComentario : idComent,
          validarEliminar : this.validar2
        }
      });
      await modal.present();

      try{
        const {data} = await modal.onDidDismiss();
        if(data.tipo ==1){
          if(data.true !=null){
            if(data.true == true){
              this.ngOnInit();
              this.presentToast('Tu comentario fue eliminado');
            }else{
              this.presentToast('El comentario no pudo ser eliminado');
            }
          }
        }else if(data.tipo==2 ){
          if(data.true !=null){
            if(data.true == true){
              this.ngOnInit();
              this.presentToast('El comentario fue reportado');
            }else{
              this.presentToast('El comentario no pudo ser reportado');
            }
          }
        }
      }catch{}        


  }
  async presentToast(message: string) {
    const toast = await this.toastCntrl.create({
      message,
      position: 'bottom',
      duration: 3000
    });
    await toast.present();
  }
    
}
