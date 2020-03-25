import { Component, OnInit } from '@angular/core';
import {  ServicioagregareService } from './servicioagregare.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-agregar',
  templateUrl: './agregar.component.html',
  styleUrls: ['./agregar.component.css']
})
export class AgregarComponent implements OnInit {
InsertarUsuario: FormGroup;
  constructor(private formBuilder: FormBuilder,
      private servi: ServicioagregareService,
      Router: Router) { }
public InsertarUser() {

    //var id= this.InsertarUsuario.getRawValue()['textid'];
    var Nombre= this.InsertarUsuario.getRawValue()['textn'];
    var FechaPublicacion = this.InsertarUsuario.getRawValue()['texta'];
    var Fecha = this.InsertarUsuario.getRawValue()['textTd'];
    var Descripcion = this.InsertarUsuario.getRawValue()['textNd'];
     var Calificacion = this.InsertarUsuario.getRawValue()['textLe'];
    var ImagenesUrl = this.InsertarUsuario.getRawValue()['textCe'];
    var ComentariosId= this.InsertarUsuario.getRawValue()['textc'];
    var ListaComentariosEvento = this.InsertarUsuario.getRawValue()['textIu'];
   
    var cadena = {"Nombre":Nombre,
    "FechaPublicacion":FechaPublicacion,"Fecha":Fecha,"Descripcion":Descripcion,
    "Calificacion":Calificacion,"ImagenesUrl":ImagenesUrl,"ComentariosId":ComentariosId,
  "ListaComentariosEvento":ListaComentariosEvento};

    this.servi.insertUsuario(cadena).then(res => {console.log(res)}).catch(err => 
      {console.log(err)});
    console.log(cadena);

  }


  ngOnInit(): void {this.InsertarUsuario = this.formBuilder.group(
    {
   
      textn: [],
       texta: [],
      textTd: [],
       textNd: [],
       textLe: [],
      textCe: [],
      textc: [],
       textIu: [],
  
      
    });

  }

}
