// Librerias
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

// Servicios
import { AuthService } from '../../services/auth.service';
import { EventoService } from '../../services/evento.service';

// Interfaces
import { Evento } from '../../interfaces/evento.interface';
import { Usuario } from '../../interfaces/usuario.interface';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.page.html',
  styleUrls: ['./eventos.page.scss'],
})
export class EventosPage implements OnInit {

  // Variables
  private usuario: Usuario;
  private session: boolean;
  private eventos: Evento[] = [];    

  constructor( // Constructor
    private eventoService: EventoService,
    private authServicio: AuthService,
    private router: Router
  ) { }

  /* Métodos */
  async ngOnInit() {
    this.session = await this.authServicio.isAuthenticated();
    await this.sessions();
    this.eventos = await this.eventoService.leerEventos();     
  }  

  async sessions() {
    if (this.session == true) {
      this.authServicio.getUsuario().then(user => {
        this.usuario = user;
      })
      console.log(this.usuario);
    }  
  }  

  verDetalles(id: number) {    
    this.router.navigate(['/detalle-evento', id]);
  }
}
