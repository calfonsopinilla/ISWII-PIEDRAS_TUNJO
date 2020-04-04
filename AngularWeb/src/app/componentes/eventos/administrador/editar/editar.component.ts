import { Component, OnInit } from '@angular/core';
import { ServicioEventoService  } from 'src/app/componentes/eventos/servicio-evento.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Evento } from 'src/app/componentes/eventos/administrador/inicio-a/evento.model';
@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrls: ['./editar.component.css']
})
export class EditarComponent implements OnInit {
 id:string;
eventos: Evento[];
  evento: Evento ={
    Nombre: '',
    FechaPublicacion: '',
     Fecha: '',
    Descripcion: '',
     Calificacion: '',
    ImagenesUrl: '',
     ComentariosId: '',
    ListaComentariosEvento: '',

   
  }
  constructor(private formBuilder: FormBuilder,
      private servi:  ServicioEventoService ,
      private Router: Router,
       private route: ActivatedRoute) { }


  



  ngOnInit(): void {
     this.id = this.route.snapshot.params['id'];
      this.servi.getu('/'+this.id).subscribe(resultado =>{
 this.evento=resultado;
  
 });


  }

     guardar({value, valid}: {value:Evento, valid: boolean}){
   
   
      value.Id = this.id;
      
   this.servi.update(value,this.id);  
      //this.route.navigate(['/']);
    
  }

  eliminar(){
     this.servi.Eliminar(this.id);
     
     this.Router.navigate(['/inicioeventos']);
   
     //this.route.navigate(['/']);
      //this.clientesServicio.eliminarCliente(this.cliente);
      //this.router.navigate(['/']);
   

}

}
