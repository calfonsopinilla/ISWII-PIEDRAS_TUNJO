import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Puntosi } from '../puntosI.model';
import { ServiciopiService  } from 'src/app/componentes/puntosInteres/serviciopi.service';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-agregarpi',
  templateUrl: './agregarpi.component.html',
  styleUrls: ['./agregarpi.component.css']
})
export class AgregarpiComponent implements OnInit {
puntosi: Puntosi[];
  puntoi: Puntosi ={
    Descripcion: '',
    Latitud: '',
     Longitud: '',
    Token: '',
     LastModification: '',
   
   
  }
  constructor(private formBuilder: FormBuilder,
      private servi:  ServiciopiService  ,
      private Router: Router,
       private route: ActivatedRoute) { }
agregar({value, valid}: {value: Puntosi, valid: boolean}){
      this.servi.insertar(value);

  }
  ngOnInit(): void {
  }

}
