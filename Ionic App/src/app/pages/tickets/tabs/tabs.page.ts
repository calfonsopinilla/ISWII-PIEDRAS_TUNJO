import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { TicketsService } from '../../../services/tickets.service';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.page.html',
  styleUrls: ['./tabs.page.scss'],
})
export class TabsPage implements OnInit {

  showInicio = false;

  constructor(
    private ticketService: TicketsService
  ) { }

  ngOnInit() {
    this.init();
  }

  async init() {
    const edad = await this.ticketService.getAgeUser();
    this.showInicio = !(edad < 5 || edad > 65);
  }

}
