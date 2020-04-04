import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ServicioLService } from 'src/app/componentes/preguntas-frecuentes/servicio-l.service';
import { Router , ActivatedRoute} from '@angular/router';
import { Pregunta } from 'src/app/componentes/preguntas-frecuentes/administrador/inicio-p/pregunta.model';
@Component({
  selector: 'app-agregar-p',
  templateUrl: './agregar-p.component.html',
  styleUrls: ['./agregar-p.component.css']
})
export class AgregarPComponent implements OnInit {
preguntas: Pregunta[];
  pregunta: Pregunta ={
    nombre: '',
    descripcion: '',
   
  }
  constructor(private formBuilder: FormBuilder,
      private servi: ServicioLService,
      Router: Router,
       ) { }
agregar({value, valid}: {value: Pregunta, valid: boolean}){
   
    
      //Agregar el nuevo cliente
      this.servi.insertar(value);
     
      
    
  }
  ngOnInit(): void {
  }

}
