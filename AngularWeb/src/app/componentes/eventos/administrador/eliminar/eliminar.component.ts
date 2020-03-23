import { Component, OnInit } from '@angular/core';
import {   ServicioeliminareService } from './servicioeliminare.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-eliminar',
  templateUrl: './eliminar.component.html',
  styleUrls: ['./eliminar.component.css']
})
export class EliminarComponent implements OnInit {

ActualizarU: FormGroup;
  constructor(private formBuilder: FormBuilder,
      private servi:  ServicioeliminareService,
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
      
    }); 
    this.formBuilder.group
  }

}
