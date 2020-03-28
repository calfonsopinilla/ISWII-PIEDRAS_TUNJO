import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { Usuarios } from '../usuarios.model';
import { ServicioUService  } from 'src/app/componentes/Usuarios/servicio-u.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-inicio-u',
  templateUrl: './inicio-u.component.html',
  styleUrls: ['./inicio-u.component.css']
})
export class InicioUComponent implements OnInit {
usuarios: Usuarios[];
  usuario: Usuarios ={
    nombre: '',
    apellido: '',
     tipoDocumento: '',
   numeroDocumento: '',
    lugarExpedicion: '',
   correoElectronico: '',
      clave: '',
    icono_url: '',
    verificacionCuenta: '',
    estadoCuenta: '',
   rolId: '',
      rolNombre: '',
    imagen_documento: '',

   
  }
  constructor(private servi:ServicioUService) {this.ObtenerUsuarios }
 ObtenerUsuarios(){
 this.servi.ObtenerJson().subscribe(resultado =>{
   this.usuarios=resultado;
   
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
