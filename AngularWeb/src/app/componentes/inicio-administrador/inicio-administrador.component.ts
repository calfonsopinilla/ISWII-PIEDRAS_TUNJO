import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { ServicioAdminService } from './servicio-admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inicio-administrador',
  templateUrl: './inicio-administrador.component.html',
  styleUrls: ['./inicio-administrador.component.css']
})
export class InicioAdministradorComponent implements OnInit {

  constructor(private servicioinfoservice:ServicioAdminService) {  this.ObtenerUsuarios}

  User:any;
  ObtenerUsuarios(){
 this.servicioinfoservice.ObtenerJson().subscribe(resultado =>{
   this.User=resultado;
   
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
  ngOnInit(): void {
  	this.ObtenerUsuarios();
  }

}
