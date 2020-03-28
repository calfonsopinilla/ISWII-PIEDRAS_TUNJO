import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { Evento } from './evento.model';
import { ServicioEventoService  } from 'src/app/componentes/eventos/servicio-evento.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-inicio-a',
  templateUrl: './inicio-a.component.html',
  styleUrls: ['./inicio-a.component.css']
})
export class InicioAComponent implements OnInit {
eventos: Evento[];
  evento: Evento ={
    Nombre: '',
    FechaPublicacion: '',
     Fecha: '',
    Descripcion: '',
     Calificacion: '',
    ImagenesUrl: '',
     ComentariosId: '',
    ListaComentariosEvento: '',

   
  }
 constructor(private servi:ServicioEventoService) {  this.ObtenerEventos}


  ObtenerEventos(){
 this.servi.ObtenerJson().subscribe(resultado =>{
   this.eventos=resultado;
   
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
  ngOnInit(): void {
  	this.ObtenerEventos();
  }

}
