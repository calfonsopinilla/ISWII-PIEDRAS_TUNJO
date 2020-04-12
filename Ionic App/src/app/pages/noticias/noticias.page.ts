import { Component, OnInit,LOCALE_ID } from '@angular/core';
import { Noticias} from '../../interfaces/noticias';
import {ServicioNoticiasService }  from '../../services/servicio-noticias.service';
import {AuthService}  from '../../services/auth.service';
import { NavController, LoadingController,NavParams} from '@ionic/angular';
import {Router,NavigationExtras} from '@angular/router';
import { NgForm } from '@angular/forms';
import { Usuario} from '../../interfaces/usuario.interface';
import { ComentarioNoticia} from '../../interfaces/comentario-noticia';
import { ModalController} from  '@ionic/angular'; 
import { ModalComentarioPage}  from '../../pages/noticias/modal-comentario/modal-comentario.page';
import { ToastController } from '@ionic/angular';


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
    private router: Router,
    private modalCtrlr : ModalController,
    private toastCntrl : ToastController
    ) { 
      this.ngOnInit();
    }
   
    comentarios: ComentarioNoticia[] = [];
    noticias : Noticias [] = [];
    noticiaSelect : Noticias ;
    session : boolean ; 
    registerUser = {comentar: ''};
    idNoticia  : string ;
    usuario : Usuario;
    datas : any ;
   async ngOnInit() {
      this.session = await this.authServicio.isAuthenticated();
      this.noticiaServicio.obtenerInformacionNoticias().subscribe(resp => {this. noticias = resp  
      });
      this.sessions();
    }
    sessions(){
      if (this.session == true) {
        this.authServicio.getUsuario().then(user => {
          this.usuario = user;
        })
        console.log(this.usuario);
    }
    
    }
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

    async agregarComentario(form: NgForm,ids){
      
      console.log(ids);
      if (form.valid == true) {
        var comentario = {}
        comentario["usuarioId"] = this.usuario.Id;
        comentario["descripcion"] = this.registerUser.comentar;
        comentario["noticia_id"] = ids;
        comentario["calificacion"] = 0;
        console.log(comentario);
        //como captar respuesta
        const resultado = await this.noticiaServicio.agregarComentario(comentario);
        console.log(resultado);
        this.registerUser.comentar = "";
        this.ngOnInit();
        
    }
    }

    validar :boolean ;
    async abrirOpciones(idComent,idUser,idUserComent){
      
      if(idUser != idUserComent){
        this.validar =  false
      }else{
        this.validar = true;
      }
      
      const modal = await this.modalCtrlr.create({ 
          component: ModalComentarioPage,
          cssClass: 'my-custom-modal-css',
          componentProps :{
            idComentario : idComent,
            validarEliminar : this.validar
          }
        });
        await modal.present();
        try{
          const {data} = await modal.onDidDismiss();
          if(data.tipo ==1){
            if(data.true !=null){
              if(data.true == true){
                //tu comentario eliminar 
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
