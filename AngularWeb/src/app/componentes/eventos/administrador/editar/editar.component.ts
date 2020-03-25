import { Component, OnInit } from '@angular/core';
import {   ServicioeditareService } from './servicioeditare.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrls: ['./editar.component.css']
})
export class EditarComponent implements OnInit {
ActualizarU: FormGroup;
  constructor(private formBuilder: FormBuilder,
      private servi:  ServicioeditareService,
      Router: Router) { }

User:any;
 Nombre:any;
   FechaPublicacion:any;
   Fecha:any;
     Descripcion:any;
   Calificacion:any;
    ImagenesUrl:any;
      ComentariosId:any;
 ListaComentariosEvento:any;
  
  ObtenerUsuario(){
     var filtovalor = this.ActualizarU.getRawValue()['textfiltro'];
     console.log(filtovalor);
 this.servi.getusuario('/'+filtovalor).subscribe(resultado =>{
   this.User=resultado;
   this.Nombre=this.User.Nombre;
    this.FechaPublicacion=this.User.FechaPublicacion;
     this.Fecha=this.User.Fecha;
       this.Descripcion=this.User.Descripcion;
    this.Calificacion=this.User.Calificacion;
     this.ImagenesUrl=this.User.ImagenesUrl;
       this.ComentariosId=this.User.ComentariosId;
    this.ListaComentariosEvento=this.User.ListaComentariosEvento;
     
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
    public ActualizarUsuario() {

    var Id = this.ActualizarU.getRawValue()['textfiltro'];
    
   var Nombre= this.ActualizarU.getRawValue()['textn'];
    var FechaPublicacion = this.ActualizarU.getRawValue()['texta'];
    var Fecha = this.ActualizarU.getRawValue()['textTd'];
    var Descripcion = this.ActualizarU.getRawValue()['textNd'];
     var Calificacion = this.ActualizarU.getRawValue()['textLe'];
    var ImagenesUrl = this.ActualizarU.getRawValue()['textCe'];
    var ComentariosId= this.ActualizarU.getRawValue()['textc'];
    var ListaComentariosEvento = this.ActualizarU.getRawValue()['textIu'];
   
    var cadena = {"Id":Id,"Nombre":Nombre,
    "FechaPublicacion":FechaPublicacion,"Fecha":Fecha,"Descripcion":Descripcion,
    "Calificacion":Calificacion,"ImagenesUrl":ImagenesUrl,"ComentariosId":ComentariosId,
  "ListaComentariosEvento":ListaComentariosEvento};
    console.log(cadena)
    this.servi.updateUsuario(cadena,Id).then
      (
        res => {
          console.log(cadena)
          console.log("res",res)
        }
      ).catch(err => {
        console.log(cadena)
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
      
    }); 
    this.formBuilder.group


  }

}
