import { Component, OnInit } from '@angular/core';
import { Ticket } from 'src/app/interfaces/ticket.interface';
import { TicketService } from 'src/app/services/ticket.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  tickets: Ticket[] = [];

  constructor(
    private ticketService: TicketService
  ) { }

  ngOnInit() {
  }

}
