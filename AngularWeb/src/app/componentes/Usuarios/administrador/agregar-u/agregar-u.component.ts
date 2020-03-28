import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { Usuarios } from '../usuarios.model';
import { ServicioUService  } from 'src/app/componentes/Usuarios/servicio-u.service';
import { Router , ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-agregar-u',
  templateUrl: './agregar-u.component.html',
  styleUrls: ['./agregar-u.component.css']
})
export class AgregarUComponent implements OnInit {
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
  constructor(private formBuilder: FormBuilder,
      private servi:  ServicioUService  ,
      private Router: Router,
       private route: ActivatedRoute) { }
  agregar({value, valid}: {value:Usuarios, valid: boolean}){
      this.servi.insertar(value);

  }

  ngOnInit(): void {
  }

}
