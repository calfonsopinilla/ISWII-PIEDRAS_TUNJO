import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { ServiciomostrareService} from './serviciomostrare.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-inicio-a',
  templateUrl: './inicio-a.component.html',
  styleUrls: ['./inicio-a.component.css']
})
export class InicioAComponent implements OnInit {

 constructor(private servicioinfoservice:ServiciomostrareService) {  this.ObtenerEventos}

  User:any;
  ObtenerEventos(){
 this.servicioinfoservice.ObtenerJson().subscribe(resultado =>{
   this.User=resultado;
   
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
