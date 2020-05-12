import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';

import { AuthService } from '../../../services/auth.service';
import { PictogramaService } from '../../../services/pictograma.service';
import { ComentarioService } from '../../../services/comentario.service';

import { Pictograma } from '../../../interfaces/pictograma.interface';
import { Usuario } from '../../../interfaces/usuario.interface';
import { Comentario } from '../../../interfaces/comentario.interface';
import { ComentarioPictograma } from '../../../interfaces/comentario-pictograma.interface';

@Component({
  selector: 'app-detalle-pictograma',
  templateUrl: './detalle-pictograma.page.html',
  styleUrls: [
    './detalle-pictograma.page.scss',
    '../../../../assets/css/comentario.css'
  ],
})
export class DetallePictogramaPage implements OnInit {

  pictograma: Pictograma;
  usuario: Usuario;
  formUser: FormGroup;  
  comentario: Comentario;
  comentarioUsuario: ComentarioPictograma;  
  listaComentariosPictograma: ComentarioPictograma[] = [];  
  lista: Pictograma[] = [];  
  estado: boolean;  
  puntuacion: Number;  

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private pictogramaService: PictogramaService,    
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
    this.lista = await this.pictogramaService.getPictogramas();
    this.lista.forEach(element => {
      if (element.Id == Number(id)) {
        this.pictograma = element;
      }
    });
    this.puntuacion = this.pictograma.Calificacion;
    if (this.pictograma != null) {      
      await this.authService.getUsuario().then(user => {
        this.usuario = user;        
      });      
      await this.leerComentariosId("pictograma",Number(id),this.usuario.Id);            
      await this.leerComentarioUsuario("pictograma",Number(id),this.usuario.Id);                  

    } else {
      this.router.navigate(['/pictogramas']);
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
    const creado = await this.comentarioService.crearComentario("pictograma",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      const id = this.route.snapshot.paramMap.get('id');    
      this.lista = await this.pictogramaService.getPictogramas();
      this.lista.forEach(element => {
        if (element.Id == Number(id)) {
          this.pictograma = element;
        }
      });
      this.puntuacion = this.pictograma.Calificacion;      
      this.presentToast("Comentario agregado correctamente");   
      this.estado = true;   
      await this.leerComentarioUsuario("pictograma",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);
    } else {
      this.presentToast("ERROR: No se pudó agregar su comentario");
    }    
    loading.dismiss();
  }

  async actualizarComentario() {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const comentario = await this.comentarioService.leerComentarioUsuario("pictograma",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);        
    this.comentario = {
      ... this.formUser.value,
      Id: comentario['id'],
      FechaPublicacion: comentario['fechaPublicacion'],
      UsuarioId: this.usuario.Id,
      LastModification: new Date(),
      Token: "Token",
      Reportado: comentario['reportado']
    };      
    const creado = await this.comentarioService.actualizarComentario("pictograma",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.presentToast("Comentario actualizado correctamente");          
      const id = this.route.snapshot.paramMap.get('id');    
      this.lista = await this.pictogramaService.getPictogramas();
      this.lista.forEach(element => {
        if (element.Id == Number(id)) {
          this.pictograma = element;
        }
      });
      this.puntuacion = this.pictograma.Calificacion;
    } else {
      this.presentToast("ERROR: No se pudó actualizar su comentario");
    }
    loading.dismiss();
  }

  async reportarComentario(comentario: any) {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const ok = await this.comentarioService.reportarComentario("pictograma", Number(this.route.snapshot.paramMap.get('id')), comentario);    
    if (ok == true) {
      this.presentToast("Comentario reportado correctamente");
      await this.leerComentariosId("pictograma",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);            
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
    this.listaComentariosPictograma = lista;    
    for (let i = 0; i < this.listaComentariosPictograma.length; i++) {      
      const comentario = this.listaComentariosPictograma[i];
      if (comentario['usuarioId'] == userId) {
        this.listaComentariosPictograma.splice(i,1);
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
