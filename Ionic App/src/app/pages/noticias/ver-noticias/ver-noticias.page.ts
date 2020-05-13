import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import {ServicioNoticiasService }  from '../../../services/servicio-noticias.service';
import {ComentarioService }  from '../../../services/comentario.service';
import {Noticias} from '../../../interfaces/noticias';
import { Usuario } from '../../../interfaces/usuario.interface';
import { Comentario } from '../../../interfaces/comentario.interface';
import {ComentarioNoticia} from '../../../interfaces/comentario-noticia.interface'; 
import { AuthService } from '../../../services/auth.service';
import { NavController, LoadingController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-ver-noticias',
  templateUrl: './ver-noticias.page.html',
  styleUrls: ['./ver-noticias.page.scss',
              '../../../../assets/css/comentario.css'],
})
export class VerNoticiasPage implements OnInit {

  formUser: FormGroup;  
  noticia : Noticias;
  usuario : Usuario;
  comentario: Comentario;
  comentarioUsuario: ComentarioNoticia;  
  listaComentariosNoticias: ComentarioNoticia[] = []; 
  estado: boolean;  
  puntuacion: Number;

  constructor(  
    private route: ActivatedRoute, 
    private fb: FormBuilder,
    public noticiaServicio : ServicioNoticiasService,
    private comentarioService : ComentarioService,
    private authService: AuthService,
    private router : Router,
    private toastCtrl :ToastController,
    private loadingCtrl: LoadingController
    )  {}

  async ngOnInit() {
    this.formUser = this.fb.group({  
      Calificacion: ['', Validators.required],
      Descripcion: ['', Validators.required]
    });   
    const id = this.route.snapshot.paramMap.get('id');
    this.noticia = await this.noticiaServicio.buscarNoticia(Number(id));  
    this.puntuacion = this.noticia.calificacion;
    this.noticia.listaImagenes = this.getImages(this.noticia.imagenesUrl);
    if (this.noticia != null) {
      await this.authService.getUsuario().then(user => {
        this.usuario = user;
      });
      await this.leerComentariosId("noticia",Number(id),this.usuario.Id);            
      await this.leerComentarioUsuario("noticia",Number(id),this.usuario.Id);                  
    } else {
      this.router.navigate(['/noticias']);
    }
  }

  async crearComentario() {        
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    this.comentario = {
      ... this.formUser.value,
      UsuarioId: this.usuario.Id,
      Id: 0,
      FechaPublicacion: new Date(),
      LastModification: new Date(),
      Token: "Token",
    };        
    const creado = await this.comentarioService.crearComentario("noticia",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.noticia = await this.noticiaServicio.buscarNoticia(Number(this.route.snapshot.paramMap.get('id')));    
      this.puntuacion = this.noticia.calificacion;
      this.noticia.listaImagenes = this.getImages(this.noticia.imagenesUrl);
      this.presentToast("Comentario agregado correctamente");      
      this.estado = true;   
      await this.leerComentarioUsuario("noticia",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);
    } else {
      this.presentToast("ERROR: No se pudó agregar su comentario");
    }    
    loading.dismiss();
  }

  async actualizarComentario() {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const comentario = await this.comentarioService.leerComentarioUsuario("noticia",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);        
    this.comentario = {
      ... this.formUser.value,
      Id: comentario['id'],
      FechaPublicacion: comentario['fechaPublicacion'],
      UsuarioId: this.usuario.Id,
      LastModification: new Date(),
      Token: "Token",
      Reportado: comentario['reportado']
    };      
    const creado = await this.comentarioService.actualizarComentario("noticia",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.presentToast("Comentario actualizado correctamente");      
      this.noticia = await this.noticiaServicio.buscarNoticia(Number(this.route.snapshot.paramMap.get('id')));    
      this.puntuacion = this.noticia.calificacion;
      this.noticia.listaImagenes = this.getImages(this.noticia.imagenesUrl);
    } else {
      this.presentToast("ERROR: No se pudó actualizar su comentario");
    }
    loading.dismiss();
  }

  async reportarComentario(comentario: any) {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const ok = await this.comentarioService.reportarComentario("noticia", Number(this.route.snapshot.paramMap.get('id')), comentario);    
    if (ok == true) {
      this.presentToast("Comentario reportado correctamente");
    } else {
      this.presentToast("ERROR: No se pudo reportar el comentario");
    }
    loading.dismiss();
  }

  async leerComentarioUsuario(table: string, objectId: number, userId: number) {
    const comentario = await this.comentarioService.leerComentarioUsuario(table,objectId,userId);
    this.comentarioUsuario = comentario;            
    if (this.comentarioUsuario == null) {
      this.estado = false;
    } else {
      this.estado = true;
    }
  }

  async leerComentariosId(table: string, objectId: number, userId: number) {
    const lista = await this.comentarioService.leerComentariosId(table,objectId);        
    this.listaComentariosNoticias = lista; 
    for (let i = 0; i < this.listaComentariosNoticias.length; i++) {      
      const comentario = this.listaComentariosNoticias[i];
      if (comentario['usuarioId'] == userId) {
        this.listaComentariosNoticias.splice(i,1);
      }
    }   
  }  

  getImages(imagenes :string) {
    const images = imagenes.split('@');
    return images;
  }      

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'bottom',
      duration: 3000
    });
    await toast.present();
  }

  get calificacion() {    
    return this.formUser.get('calificacion');
  }    

  get descripcion() {
    return this.formUser.get('descripcion');
  }

}
