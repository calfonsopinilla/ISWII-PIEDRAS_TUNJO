import { Component, OnInit } from '@angular/core';
import { ServicioeliminarUService } from './servicioeliminar-u.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-eliminar-usuario',
  templateUrl: './eliminar-usuario.component.html',
  styleUrls: ['./eliminar-usuario.component.css']
})
export class EliminarUsuarioComponent implements OnInit {
ActualizarU: FormGroup;
  constructor(private formBuilder: FormBuilder,
      private servi:  ServicioeliminarUService,
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
    
   

   
    
    this.servi.updateUsuario(Id).then
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
