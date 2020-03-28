import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ServicioLService } from 'src/app/componentes/preguntas-frecuentes/servicio-l.service';
import { Router , ActivatedRoute} from '@angular/router';
import { Pregunta } from 'src/app/componentes/preguntas-frecuentes/administrador/inicio-p/pregunta.model';
@Component({
  selector: 'app-editar-p',
  templateUrl: './editar-p.component.html',
  styleUrls: ['./editar-p.component.css']
})
export class EditarPComponent implements OnInit {
  id:string;
  preguntas: Pregunta[];
  pregunta: Pregunta ={
    nombre: '',
    descripcion: '',
   
  }
  constructor(private formBuilder: FormBuilder,
      private servi: ServicioLService,
      private Router: Router,
       private route: ActivatedRoute  ) { }
 
  
  ngOnInit(): void {
  	 this.id = this.route.snapshot.params['id'];
  	  this.servi.getu('/'+this.id).subscribe(resultado =>{
 this.pregunta=resultado;
  
 });
  }
   guardar({value, valid}: {value: Pregunta, valid: boolean}){
   
   
      value.id = this.id;
      
   this.servi.update(value,this.id);  
      //this.route.navigate(['/']);
    
  }

  eliminar(){
     this.servi.Eliminar(this.id);
     
     this.Router.navigate(['/inicioaPf']);
   
     //this.route.navigate(['/']);
      //this.clientesServicio.eliminarCliente(this.cliente);
      //this.router.navigate(['/']);
   

}
}
