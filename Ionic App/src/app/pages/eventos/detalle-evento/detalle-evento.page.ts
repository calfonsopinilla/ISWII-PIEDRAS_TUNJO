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
  evento: Evento;
  usuario: Usuario;
  formUser: FormGroup;  
  comentario: Comentario;
  comentarioUsuario: ComentarioEvento;  
  listaComentariosEvento: ComentarioEvento[] = [];  
  estado: boolean;  
  puntuacion: Number;  

  constructor(    
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private eventoService: EventoService,    
    private comentarioService: ComentarioService,
    private toastCtrl: ToastController,
    private loadingCtrl: LoadingController
  ) { }

  async ngOnInit() {    
    
    this.formUser = this.fb.group({  
      Calificacion: ['', Validators.required],
      Descripcion: ['', Validators.required]
    });    
    const id = this.route.snapshot.paramMap.get('id');    
    this.evento = await this.eventoService.buscarEvento(Number(id));    
    this.puntuacion = this.evento.Calificacion;
    if (this.evento != null) {      
      await this.authService.getUsuario().then(user => {
        this.usuario = user;        
      });      
      await this.leerComentariosId("evento",Number(id),this.usuario.Id);            
      await this.leerComentarioUsuario("evento",Number(id),this.usuario.Id);                  

    } else {
      this.router.navigate(['/eventos']);
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
    const creado = await this.comentarioService.crearComentario("evento",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.evento = await this.eventoService.buscarEvento(Number(this.route.snapshot.paramMap.get('id')));    
      this.puntuacion = this.evento.Calificacion;
      this.presentToast("Comentario agregado correctamente");   
      this.estado = true;   
      await this.leerComentarioUsuario("evento",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);
    } else {
      this.presentToast("ERROR: No se pudó agregar su comentario");
    }    
    loading.dismiss();
  }

  async actualizarComentario() {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
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
      this.evento = await this.eventoService.buscarEvento(Number(this.route.snapshot.paramMap.get('id')));    
      this.puntuacion = this.evento.Calificacion;
    } else {
      this.presentToast("ERROR: No se pudó actualizar su comentario");
    }
    loading.dismiss();
  }

  async reportarComentario(comentario: any) {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const ok = await this.comentarioService.reportarComentario("evento", Number(this.route.snapshot.paramMap.get('id')), comentario);    
    if (ok == true) {
      this.presentToast("Comentario reportado correctamente");
      await this.leerComentariosId("evento",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);            
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
    this.listaComentariosEvento = lista;    
    for (let i = 0; i < this.listaComentariosEvento.length; i++) {      
      const comentario = this.listaComentariosEvento[i];
      if (comentario['usuarioId'] == userId) {
        this.listaComentariosEvento.splice(i,1);
      }
    }
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