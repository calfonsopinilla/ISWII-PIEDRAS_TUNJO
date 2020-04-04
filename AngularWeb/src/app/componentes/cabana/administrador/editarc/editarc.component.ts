import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { Cabana } from '../cabana.model';
import { ServiciocService  } from 'src/app/componentes/cabana/servicioc.service';
import { Router ,ActivatedRoute} from '@angular/router';
@Component({
  selector: 'app-editarc',
  templateUrl: './editarc.component.html',
  styleUrls: ['./editarc.component.css']
})
export class EditarcComponent implements OnInit {
	id:string;
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
  	    this.id = this.route.snapshot.params['id'];
      this.servi.getu('/'+this.id).subscribe(resultado =>{
 this.cabana=resultado;
  
 });
  }
      guardar({value, valid}: {value:Cabana, valid: boolean}){
   
   
      value.id = this.id;
      
   this.servi.update(value,this.id);  
      //this.route.navigate(['/']);
    
  }

  eliminar(){
     this.servi.Eliminar(this.id);
     
     this.Router.navigate(['/inicioc']);
   
     //this.route.navigate(['/']);
      //this.clientesServicio.eliminarCliente(this.cliente);
      //this.router.navigate(['/']);
   

}

}
