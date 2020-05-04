import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';

import { AuthService } from '../../../services/auth.service';
import { EventoService } from '../../../services/evento.service';
import { ComentarioService } from '../../../services/comentario.service';

import { Evento } from '../../../interfaces/evento.interface';
import { Usuario } from '../../../interfaces/usuario.interface';
import { Comentario } from '../../../interfaces/comentario.interface';
import { ComentarioEvento } from '../../../interfaces/comentario-evento.interface';

@Component({
  selector: 'app-detalle-evento',
  templateUrl: './detalle-evento.page.html',
  styleUrls: [
    './detalle-evento.page.scss',    
    '../../../../assets/css/comentario.css'
  ],
})
export class DetalleEventoPage implements OnInit {
    
  private evento: Evento;
  private usuario: Usuario;
  private formUser: FormGroup;  
  private comentario: Comentario;
  private comentarioUsuario: ComentarioEvento;  
  private listaComentariosEvento: ComentarioEvento[] = [];  
  private estado: boolean;  

  constructor(    
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private eventoService: EventoService,    
    private comentarioService: ComentarioService,
    private toastCtrl: ToastController
  ) { }

  async ngOnInit() {    
    
    this.formUser = this.fb.group({  
      Calificacion: ['', Validators.required],
      Descripcion: ['', Validators.required]
    });    
    const id = this.route.snapshot.paramMap.get('id');    
    this.evento = await this.eventoService.buscarEvento(Number(id));    
    if (this.evento != null) {
      await this.leerComentariosId("evento",Number(id));      
      await this.authService.getUsuario().then(user => {
        this.usuario = user;        
      });
      await this.leerComentarioUsuario("evento",Number(id),this.usuario.Id);                  

    } else {
      this.router.navigate(['/eventos']);
    }
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

  async leerComentarioUsuario(table: string, objectId: number, userId: number) {
    const comentario = await this.comentarioService.leerComentarioUsuario(table,objectId,userId);
    this.comentarioUsuario = comentario;            
    if (this.comentarioUsuario == null) {
      this.estado = false;
    } else {
      this.estado = true;
    }
  }

  async leerComentariosId(table: string, objectId: number) {
    const lista = await this.comentarioService.leerComentariosId(table,objectId);        
    this.listaComentariosEvento = lista;    
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