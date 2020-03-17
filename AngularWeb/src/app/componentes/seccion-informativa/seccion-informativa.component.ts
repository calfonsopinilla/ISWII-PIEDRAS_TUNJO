import { Component, OnInit , ComponentFactoryResolver} from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { ServicioInfoService } from './servicio-info.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-seccion-informativa',
  templateUrl: './seccion-informativa.component.html',
  styleUrls: ['./seccion-informativa.component.css']
})
export class SeccionInformativaComponent implements OnInit {

  constructor(private servicioinfoservice:ServicioInfoService) {  this.ObtenerInformacion}
  Informacion:any;
  descripcion:any;
  imagen :any;
  ObtenerInformacion(){
 this.servicioinfoservice.ObtenerJson().subscribe(resultado =>{
   this.Informacion=resultado;
   this.descripcion=this.Informacion.descripcion;
  
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }
  ngOnInit(): void {
  	this.ObtenerInformacion();

}
}
