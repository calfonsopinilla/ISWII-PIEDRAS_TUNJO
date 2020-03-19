import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule , FormsModule } from '@angular/forms';
import { ServicioxService } from './serviciox.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-componentex',
  templateUrl: './componentex.component.html',
  styleUrls: ['./componentex.component.css']
})
export class ComponentexComponent implements OnInit {
 User:any;
  constructor(private serviciox: ServicioxService) { this.ObtenerUsuarios}
   ObtenerUsuarios(){
 this.serviciox.ObtenerJson().subscribe(resultado =>{
   this.User=resultado;
   
   console.log("Informacion ya tiene resultado");
  
 },
 error=>{
console.log(JSON.stringify(error));

 }); 
   }

  ngOnInit(): void {
  	this.ObtenerUsuarios();
  }

}
