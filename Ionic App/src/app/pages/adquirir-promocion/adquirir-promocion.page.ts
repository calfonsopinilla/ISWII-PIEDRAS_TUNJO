import { Component, OnInit } from '@angular/core';
import { Router,ActivatedRoute, Route} from '@angular/router';
import {Promocion} from '../../interfaces/promocion';

@Component({
  selector: 'app-adquirir-promocion',
  templateUrl: './adquirir-promocion.page.html',
  styleUrls: ['./adquirir-promocion.page.scss'],
})
export class AdquirirPromocionPage implements OnInit {

  promocion : Promocion;
  data : any ;
  
  
  constructor(private activeRoute :ActivatedRoute ,private router: Router ) { 
    this.activeRoute.queryParams.subscribe(params => {
      console.log('params: ',params);
        if(params && params.special){
            this.data = params.special;
        }
    })
  }
  ngOnInit() {
    this.promocion = JSON.parse(this.data);
      console.log(this.promocion);
  }

}
