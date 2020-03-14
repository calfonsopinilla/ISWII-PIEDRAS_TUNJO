import { Component, OnInit ,ComponentFactoryResolver } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { ServicioEventoService } from './servicio-evento.service';
import { Router } from '@angular/router';

import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {


	  fecha=Date.now();

    
    public minDate: Date = new Date(this.fecha);
    public maxDate: Date = new Date ("08/27/2020");
     public value: Date = new Date (this.fecha);
  Eventos:any;

 constructor(private servicioeventoservice:ServicioEventoService) { this.ObtenerEventos}
   ObtenerEventos(){
 this.servicioeventoservice.ObtenerJson().subscribe(resultado =>{
   this.Eventos=resultado;
   console.log("evntos ya tiene resultado");
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
  ngOnInit(): void {
  }

}
