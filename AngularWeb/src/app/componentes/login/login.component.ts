import { Component, OnInit } from '@angular/core';
import{Router} from '@angular/router';

import { FormBuilder, FormGroup,Validators, ReactiveFormsModule } from '@angular/forms';
import { IUserInfo } from './user-info';
import { ServiciologinService} from './serviciologin.service'
import { isError } from 'util';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
email:string;
password:string;

  constructor(private fb: FormBuilder,
    private accountService: ServiciologinService,
    private router: Router) {

   }
  

  ngOnInit(): void {
  	
  }
 
 
login(){
console.log("usuario registrado");

this.router.navigateByUrl('/inicioadministrador');
}
}
