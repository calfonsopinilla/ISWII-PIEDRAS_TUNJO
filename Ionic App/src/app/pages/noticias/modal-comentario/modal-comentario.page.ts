import { Component, OnInit,Input } from '@angular/core';
import { Router } from '@angular/router';
import { ModalController} from '@ionic/angular';
import { ignoreElements } from 'rxjs/operators';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import {ServicioNoticiasService}  from '../../../services/servicio-noticias.service';


@Component({
  selector: 'app-modal-comentario',
  templateUrl: './modal-comentario.page.html',
  styleUrls: ['./modal-comentario.page.scss'],
})
export class ModalComentarioPage implements OnInit {

  @Input() idComentario;

  @Input() validarEliminar
  constructor( private servicioNoticias : ServicioNoticiasService,
                private router : Router,
                private modalCtlr : ModalController            
    ) { }
  
  ngOnInit() {
    
  }
 resultado : boolean = null;
  async eliminarComentario(){
    const id = this.idComentario;
    this.resultado= await this.servicioNoticias.eliminarComentario(id);
    this.modalCtlr.dismiss({true:this.resultado,tipo:1});
  } 

  async reportarComentario(){
    const id = this.idComentario;
    this.resultado= await this.servicioNoticias.reportarComentario(id);
    this.modalCtlr.dismiss({true:this.resultado,tipo:2});
  }

}
