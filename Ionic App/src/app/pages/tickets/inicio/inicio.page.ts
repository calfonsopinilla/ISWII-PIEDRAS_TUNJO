import { Component, OnInit } from '@angular/core';
import { ReservaTicketService } from 'src/app/services/reserva-tickets.service';
import { ReservaTicket } from 'src/app/interfaces/reserva-ticket.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.page.html',
  styleUrls: ['./inicio.page.scss'],
})
export class InicioPage implements OnInit {

  reservas: ReservaTicket[] = [];
  slidesOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  constructor(
    private reservaTicketService: ReservaTicketService,
    private router: Router
  ) { }

  ngOnInit() {
    // Reserva eliminada
    this.reservaTicketService.reservaEliminada$.subscribe((id: number) => {
      // console.log(id);
      this.obtenerReservas();
    });
    // Nueva reserva
    this.reservaTicketService.nuevaReserva$.subscribe((res: ReservaTicket) => {
      this.obtenerReservas();
    });

    this.obtenerReservas();
  }

  async obtenerReservas() {
    this.reservas = await this.reservaTicketService.getTicketsUser();
    console.log(this.reservas);
  }

  verDetalles(id: number) {
    // console.log(id);
    this.router.navigate(['/tickets', 'detalle-ticket', id]);
  }

}
