import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Puntosi } from '../puntosI.model';
import { ServiciopiService  } from 'src/app/componentes/puntosInteres/serviciopi.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-editarpi',
  templateUrl: './editarpi.component.html',
  styleUrls: ['./editarpi.component.css']
})
export class EditarpiComponent implements OnInit {
id:string;
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

  ngOnInit(): void {
  this.id = this.route.snapshot.params['id'];
      this.servi.getu('/'+this.id).subscribe(resultado =>{
 this.puntoi=resultado;
  console.log(this.puntoi);
 });


  }
guardar({value, valid}: {value:Puntosi, valid: boolean}){
   
   
      value.Id = this.id;
      
   this.servi.update(value,this.id);  
      //this.route.navigate(['/']);
    
  }

  eliminar(){
     this.servi.Eliminar(this.id);
     
     this.Router.navigate(['/iniciopi']);
   
     //this.route.navigate(['/']);
      //this.clientesServicio.eliminarCliente(this.cliente);
      //this.router.navigate(['/']);
   

}
}
