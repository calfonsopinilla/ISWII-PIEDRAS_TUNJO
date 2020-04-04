import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { Cabana } from '../cabana.model';
import { ServiciocService  } from 'src/app/componentes/cabana/servicioc.service';
import { Router ,ActivatedRoute} from '@angular/router';
@Component({
  selector: 'app-agregarc',
  templateUrl: './agregarc.component.html',
  styleUrls: ['./agregarc.component.css']
})
export class AgregarcComponent implements OnInit {
cabanas: Cabana[];
  cabana: Cabana ={
    nombre: '',
    capacidad: '',
    precio: '',
    calificacion: '',
     imagenesUrl: '',
    comentariosId: '',
     token: '',
    listaComentariosCabana: '',

   
  }
  constructor(private formBuilder: FormBuilder,
      private servi:  ServiciocService ,
      private Router: Router,
       private route: ActivatedRoute) { }

  ngOnInit(): void {
  }
  agregar({value, valid}: {value: Cabana, valid: boolean}){
      this.servi.insertar(value);

  }

}
