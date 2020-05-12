import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavController, LoadingController, ToastController } from '@ionic/angular';

// Servicios
import { AuthService } from '../../../services/auth.service';
import { CabanaService } from '../../../services/cabana.service';
import { ComentarioService } from '../../../services/comentario.service';

// Interfaces
import { Cabana } from '../../../interfaces/cabana.interface';
import { Usuario } from '../../../interfaces/usuario.interface';
import { Comentario } from '../../../interfaces/comentario.interface';
import { ComentarioCabana } from '../../../interfaces/comentario-cabana.interface';

@Component({
  selector: 'app-detalles-cabana',
  templateUrl: './detalles-cabana.page.html',
  styleUrls: ['./detalles-cabana.page.scss'],
})
export class DetallesCabanaPage implements OnInit {

  cabana: Cabana = undefined;
  usuario: Usuario;
  formUser: FormGroup;  
  comentario: Comentario;
  comentarioUsuario: ComentarioCabana;  
  listaComentariosCabana: ComentarioCabana[] = [];  
  estado: boolean;  
  puntuacion: Number;  

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastCtrl: ToastController,
    private cabanaService: CabanaService,        
    private loadingCtrl: LoadingController,
    private comentarioService: ComentarioService
  ) { }

  async ngOnInit() {
    this.formUser = this.fb.group({  
      Calificacion: ['', Validators.required],
      Descripcion: ['', Validators.required]
    });   
    await this.cargarCabana();
    this.puntuacion = this.cabana.Calificacion;
    if (this.cabana != null) {      
      await this.authService.getUsuario().then(user => {
        this.usuario = user;        
      });      
      const id = this.route.snapshot.paramMap.get('id');    
      await this.leerComentariosId("cabana",Number(id),this.usuario.Id);            
      await this.leerComentarioUsuario("cabana",Number(id),this.usuario.Id);                  

    } else {
      this.router.navigate(['/cabanas']);
    }
  }

  async cargarCabana() {
    const id = this.route.snapshot.paramMap.get('id');
    this.cabana = await this.cabanaService.getCabana(Number(id));
    // console.log(this.cabana);
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
    const creado = await this.comentarioService.crearComentario("cabana",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.cargarCabana();
      this.puntuacion = this.cabana.Calificacion;
      this.presentToast("Comentario agregado correctamente");   
      this.estado = true;   
      await this.leerComentarioUsuario("cabana",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);
    } else {
      this.presentToast("ERROR: No se pudó agregar su comentario");
    }    
    loading.dismiss();
  }

  async actualizarComentario() {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const comentario = await this.comentarioService.leerComentarioUsuario("cabana",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);        
    this.comentario = {
      ... this.formUser.value,
      Id: comentario['id'],
      FechaPublicacion: comentario['fechaPublicacion'],
      UsuarioId: this.usuario.Id,
      LastModification: new Date(),
      Token: "Token",
      Reportado: comentario['reportado']
    };      
    const creado = await this.comentarioService.actualizarComentario("cabana",Number(this.route.snapshot.paramMap.get('id')),this.comentario);
    if (creado) {
      this.presentToast("Comentario actualizado correctamente");          
      this.cargarCabana();
      this.puntuacion = this.cabana.Calificacion;
    } else {
      this.presentToast("ERROR: No se pudó actualizar su comentario");
    }
    loading.dismiss();
  }

  async reportarComentario(comentario: any) {    
    const loading = await this.loadingCtrl.create({ message: 'Espere por favor' });
    await loading.present();        
    const ok = await this.comentarioService.reportarComentario("cabana", Number(this.route.snapshot.paramMap.get('id')), comentario);    
    if (ok == true) {
      this.presentToast("Comentario reportado correctamente");
      await this.leerComentariosId("cabana",Number(this.route.snapshot.paramMap.get('id')),this.usuario.Id);            
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
    this.listaComentariosCabana = lista;    
    for (let i = 0; i < this.listaComentariosCabana.length; i++) {      
      const comentario = this.listaComentariosCabana[i];
      if (comentario['usuarioId'] == userId) {
        this.listaComentariosCabana.splice(i,1);
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
