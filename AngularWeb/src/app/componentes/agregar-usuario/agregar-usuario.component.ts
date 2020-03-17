import { Component, OnInit } from '@angular/core';
import { ServicioInsertService } from './servicio-insert.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-agregar-usuario',
  templateUrl: './agregar-usuario.component.html',
  styleUrls: ['./agregar-usuario.component.css']
})
export class AgregarUsuarioComponent implements OnInit {
primerNombre:string;
segundoNombre:string;
primerApellido:string;
segundoApellido:string;
email:string;
clave:string;
numeroDocumento:string;
LugarE:string;
RolId:number;
NombreRol:string;
  InsertarUsuario: FormGroup;
  constructor( private formBuilder: FormBuilder,
      private servi: ServicioInsertService,
      Router: Router) { }
public InsertarU() {

    var nombre = this.InsertarUsuario.getRawValue()['nombre'];
    var apellido= this.InsertarUsuario.getRawValue()['apellido'];

    var cadena = {"nombre":nombre,"apellido":apellido};

    this.servi.insertUsuario(cadena).then(res => {console.log(res)}).catch(err => 
      {console.log(err)});
  }
  ngOnInit(): void {
  }

}
