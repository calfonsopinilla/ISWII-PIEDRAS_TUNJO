import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
	  fecha=Date.now();
     
 public minDate: Date = new Date(this.fecha);
    public maxDate: Date = new Date ("08/27/2020");

    public value: Date = new Date (this.fecha);
  title='fechaFutura';
 
  constructor() { }

  ngOnInit(): void {
  }

}
