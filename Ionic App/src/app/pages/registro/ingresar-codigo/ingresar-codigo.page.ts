import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-ingresar-codigo',
  templateUrl: './ingresar-codigo.page.html',
  styleUrls: ['./ingresar-codigo.page.scss'],
})
export class IngresarCodigoPage implements OnInit {

  // Variables
  code: string = "";
  error: boolean = false;
  ok: boolean;

  constructor(    
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit() {
    
  }

  validateCode() {

    if (this.code.length != 6) {
      this.error = true;
    } else {
      this.error = false;
      this.userService.validarCodigo(this.code).then(resp => { this.ok = resp['ok'] });
      if (this.ok) {
        this.router.navigate(['/login']);
      }
    }    
  }

}
