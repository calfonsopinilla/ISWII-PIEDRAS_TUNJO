import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-editar-cliente',
  templateUrl: './editar-cliente.component.html',
  styleUrls: ['./editar-cliente.component.css']
})
export class EditarClienteComponent implements OnInit {
 nombreInput:string;
  apellidoInput:string;
   CedulaInput:string;
  CorreoInput:string;
  constructor() { }
onAgregarPersona(){
  	//let persona1= new Persona(this.nombreInput , this.apellidoInput);
  	//this.personas.push(persona1);
  //	this.personaCreada.emit(persona1);
  }
  ngOnInit(): void {
  }

}
