import { Component, OnInit  , ComponentFactoryResolver} from '@angular/core';
import { ServicioInsertService } from './servicio-insert.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-agregar-usuario',
  templateUrl: './agregar-usuario.component.html',
  styleUrls: ['./agregar-usuario.component.css']
})
export class AgregarUsuarioComponent implements OnInit {


  InsertarUsuario: FormGroup;
  constructor( private formBuilder: FormBuilder,
      private servi: ServicioInsertService,
      Router: Router) { }

public InsertarUser() {

    //var id= this.InsertarUsuario.getRawValue()['textid'];
    var nombre = this.InsertarUsuario.getRawValue()['textn'];
    var apellido = this.InsertarUsuario.getRawValue()['texta'];
    var tipoDocumento = this.InsertarUsuario.getRawValue()['textTd'];
    var numeroDocumento = this.InsertarUsuario.getRawValue()['textNd'];
     var lugarExpedicion = this.InsertarUsuario.getRawValue()['textLe'];
    var correoElectronico = this.InsertarUsuario.getRawValue()['textCe'];
    var clave = this.InsertarUsuario.getRawValue()['textc'];
    var icono_url = this.InsertarUsuario.getRawValue()['textIu'];
     var verificacionCuenta = this.InsertarUsuario.getRawValue()['textVc'];
    var estadoCuenta = this.InsertarUsuario.getRawValue()['textEc'];
    var rolId = this.InsertarUsuario.getRawValue()['textRi'];
    var rolNombre = this.InsertarUsuario.getRawValue()['textRn'];
    var imagen_documento = this.InsertarUsuario.getRawValue()['textId'];
    var token = this.InsertarUsuario.getRawValue()['textT'];

    var cadena = {"nombre":nombre,
    "apellido":apellido,"tipoDocumento":tipoDocumento,"numeroDocumento":numeroDocumento,
    "lugarExpedicion":lugarExpedicion,"correoElectronico":correoElectronico,"clave":clave,
  "icono_url":icono_url,"verificacionCuenta":verificacionCuenta,"estadoCuenta":estadoCuenta,
"rolId":rolId,"rolNombre":rolNombre,"imagen_documento":imagen_documento};

    this.servi.insertUsuario(cadena).then(res => {console.log(res)}).catch(err => 
      {console.log(err)});
    console.log(cadena);

  }


  
  ngOnInit(): void {
      this.InsertarUsuario = this.formBuilder.group(
    {
   
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
  }

}
