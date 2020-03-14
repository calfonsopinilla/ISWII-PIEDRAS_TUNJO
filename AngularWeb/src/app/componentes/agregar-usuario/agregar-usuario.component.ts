import { Component, OnInit } from '@angular/core';

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
  constructor() { }
registro(){

}
  ngOnInit(): void {
  }

}
