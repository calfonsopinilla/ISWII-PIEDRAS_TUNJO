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
  constructor(  
    private route: ActivatedRoute, 
    private fb: FormBuilder,
    public noticiaServicio : ServicioNoticiasService,
    private comentarioService : ComentarioService,
    private authService: AuthService,
    private router : Router,
    private toastCtrl :ToastController
    )  {}
  async ngOnInit() {
    this.formUser = this.fb.group({  
      Calificacion: ['', Validators.required],
      Descripcion: ['', Validators.required]
    });   
    const id = this.route.snapshot.paramMap.get('id');
    this.noticia = await this.noticiaServicio.buscarNoticia(Number(id));  
    this.noticia.listaImagenes = this.getImages(this.noticia.imagenesUrl);
    if (this.noticia != null) {
      await this.leerComentariosId("noticia",Number(id));      
      await this.authService.getUsuario().then(user => {
        this.usuario = user;
      });
      await this.leerComentarioUsuario("noticia",Number(id),this.usuario.Id);                  
    } else {
      this.router.navigate(['/noticias']);
    }
  }

  async leerComentariosId(table: string, objectId: number) {
    const lista = await this.comentarioService.leerComentariosId(table,objectId);        
    this.listaComentariosNoticias = lista;    
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

  getImages(imagenes :string) {
    const images = imagenes.split('@');
    return images;
  }


  async crearComentario() {        
    this.comentario = {
      ... this.formUser.value,
      UsuarioId: this.usuario.Id,
      Id: 0,
      FechaPublicacion: new Date(),
      LastModification: new Date(),
      Token: "Token",
    };        
    const creado = await this.comentarioService.crearComentario("evento",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.presentToast("Comentario agregado correctamente");      
    } else {
      this.presentToast("ERROR: No se pudó agregar su comentario");
    }    
  }

  async actualizarComentario() {    
    const comentario = await this.comentarioService.leerComentarioUsuario("evento",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);        
    this.comentario = {
      ... this.formUser.value,
      Id: comentario['id'],
      FechaPublicacion: comentario['fechaPublicacion'],
      UsuarioId: this.usuario.Id,
      LastModification: new Date(),
      Token: "Token",
      Reportado: comentario['reportado']
    };      
    const creado = await this.comentarioService.actualizarComentario("evento",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.presentToast("Comentario actualizado correctamente");      
    } else {
      this.presentToast("ERROR: No se pudó actualizar su comentario");
    }
  }

  async reportarComentario(comentario: any) {    
    const ok = await this.comentarioService.reportarComentario("evento", 1, comentario);    
    if (ok == true) {
      this.presentToast("Comentario reportado correctamente");
    } else {
      this.presentToast("ERROR: No se pudo reportar el comentario");
    }
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'top',
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
