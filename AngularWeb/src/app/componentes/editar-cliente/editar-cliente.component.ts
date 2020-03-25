import { Component, OnInit } from '@angular/core';
import {  ServicioEditarService } from './servicio-editar.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-editar-cliente',
  templateUrl: './editar-cliente.component.html',
  styleUrls: ['./editar-cliente.component.css']
})
export class EditarClienteComponent implements OnInit {

ActualizarU: FormGroup;




  constructor(private formBuilder: FormBuilder,
      private servi: ServicioEditarService,
      Router: Router) { }

  User:any;
  nombre:any;
   apellido:any;
   tipoDocumento:any;
     numeroDocumento:any;
   lugarExpedicion:any;
    correoElectronico:any;
      clave:any;
  icono_url:any;
    verificacionCuenta:any;
     estadoCuenta:any;
   rolId:any;
    rolNombre:any;
   imagen_documento:any;
  token:any;
  ObtenerUsuario(){
     var filtovalor = this.ActualizarU.getRawValue()['textfiltro'];
     console.log(filtovalor);
 this.servi.getusuario('/'+filtovalor).subscribe(resultado =>{
   this.User=resultado;
   this.nombre=this.User.nombre;
    this.apellido=this.User.apellido;
     this.tipoDocumento=this.User.tipoDocumento;
       this.numeroDocumento=this.User.numeroDocumento;
    this.lugarExpedicion=this.User.lugarExpedicion;
     this.correoElectronico=this.User.correoElectronico;
       this.clave=this.User.clave;
    this.icono_url=this.User.icono_url;
     this.verificacionCuenta=this.User.verificacionCuenta;
       this.estadoCuenta=this.User.estadoCuenta;
    this.rolId=this.User.rolId;
     this.rolNombre=this.User.rolNombre;
     this.imagen_documento=this.User.imagen_documento;
this.token=this.User.token;
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
    public ActualizarUsuario() {

    var Id = this.ActualizarU.getRawValue()['textfiltro'];
    
    var nombre = this.ActualizarU.getRawValue()['textn'];
    var apellido = this.ActualizarU.getRawValue()['texta'];
    var tipoDocumento = this.ActualizarU.getRawValue()['textTd'];
    var numeroDocumento = this.ActualizarU.getRawValue()['textNd'];
     var lugarExpedicion = this.ActualizarU.getRawValue()['textLe'];
    var correoElectronico = this.ActualizarU.getRawValue()['textCe'];
    var clave = this.ActualizarU.getRawValue()['textc'];
    var icono_url = this.ActualizarU.getRawValue()['textIu'];
     var verificacionCuenta = this.ActualizarU.getRawValue()['textVc'];
    var estadoCuenta = this.ActualizarU.getRawValue()['textEc'];
    var rolId = this.ActualizarU.getRawValue()['textRi'];
    var rolNombre = this.ActualizarU.getRawValue()['textRn'];
    var imagen_documento = this.ActualizarU.getRawValue()['textId'];
    var token = this.ActualizarU.getRawValue()['textT'];

    var cadena = { "id": Id,"nombre":nombre,
    "apellido":apellido,"tipoDocumento":tipoDocumento,"numeroDocumento":numeroDocumento,
    "lugarExpedicion":lugarExpedicion,"correoElectronico":correoElectronico,"clave":clave,
  "icono_url":icono_url,"verificacionCuenta":verificacionCuenta,"estadoCuenta":estadoCuenta,
"rolId":rolId,"rolNombre":rolNombre,"imagen_documento":imagen_documento};
    
    this.servi.updateUsuario(cadena,Id).then
      (
        res => {
          console.log("res",res)
        }
      ).catch(err => {
        console.log(err)
      })
  } 
     
  ngOnInit(): void {
    this.ActualizarU = this.formBuilder.group(
    {
      textfiltro: [],
      textn: [],
       texta: [],
      textTd: [],
       textNd: [],
       textLe: [],
      textCe: [],
      textc: [],
       textIu: [],
      textVc: [],
      textEc: [],
       textRi: [],
      textRn: [],
        textId: [],
      textT: [],
    }); 
    this.formBuilder.group
 

}
}