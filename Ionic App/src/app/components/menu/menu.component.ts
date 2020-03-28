import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {

  itemsParque = [
    {
      label: 'Inicio',
      ruta: '/inicio',
      icon: 'home-outline'
    },
    {
      label: 'Descripción',
      ruta: '/descripcion-parque/2',
      icon: 'book-outline'
    },
    {
      label: 'Reseña histórica',
      ruta: '/resenia-historica/4',
      icon: 'book-outline'
    },
    {
      label: 'Ubicación',
      ruta: '/ubicacion-parque/5',
      icon: 'location-outline'
    },
    {
      label: 'Piedras',
      ruta: '/piedras-parque/6',
      icon: 'aperture-outline'
    },
    {
      label: 'PQR´s',
      ruta: '/pqr-parque',
      icon: 'help-circle-outline'
    }
  ];

  itemsServicios = [
    {
      label: 'Tickets',
      ruta: '/tickets',
      icon: 'map-outline'
    },
    {
      label: 'Reserva de Cabañas',
      ruta: '/reserva-cabanias',
      icon: 'calendar-outline'
    },
    {
      label: 'Reserva de Canoas',
      ruta: '/reserva-cabañas',
      icon: 'boat-outline'
    },
  ];

  itemsCuenta = [
    {
      label: 'Iniciar sesión',
      ruta: '/login',
      icon: 'log-in-outline'
    },
    {
      label: 'Registrarse',
      ruta: '/registro',
      icon: 'disc-outline'
    },
  ];

  auth: boolean;

  constructor(
    public authService: AuthService,
    private toastCtrl: ToastController
  ) { }

  async ngOnInit() {
    this.auth = await this.authService.isAuthenticated();
    // console.log('Logged: ', this.auth);
  }

  async logout() {
    const toast = await this.toastCtrl.create({
      message: 'Sesión cerrada',
      duration: 3000
    });
    this.authService.logout();
    await toast.present();
  }

}
