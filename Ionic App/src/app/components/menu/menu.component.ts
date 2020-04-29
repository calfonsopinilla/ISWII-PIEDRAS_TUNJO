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
      icon: 'home'
    },
    {
      label: 'Descripción',
      ruta: '/descripcion-parque/2',
      icon: 'book'
    },
    {
      label: 'Reseña histórica',
      ruta: '/resenia-historica/4',
      icon: 'book'
    },
    {
      label: 'Ubicación',
      ruta: '/ubicacion-parque/5',
      icon: 'location'
    },
    {
      label: 'Piedras',
      ruta: '/piedras-parque/6',
      icon: 'aperture'
    },
    {
      label: 'PQR´s',
      ruta: '/pqr-parque',
      icon: 'help-circle'
    },
    {
      label: 'Preguntas frecuentes',
      ruta: '/preguntas-frecuentes',
      icon: 'help-circle'
    }
  ];

  itemsServicios = [
    {
      label: 'Recorridos',
      ruta: '/recorridos',
      icon: 'compass'
    },
    {
      label: 'Pictogramas',
      ruta: '/pictogramas',
      icon: 'bonfire'
    },
    {
      label: 'Tickets',
      ruta: '/tickets',
      icon: 'map'
    },
    {
      label: 'Reserva de Cabañas',
      ruta: '/cabanas',
      icon: 'calendar'
    },
    {
      label : 'Promociones',
      ruta : '/promociones',
      icon : 'clipboard'
    },
    {
      label : 'Noticias',
      ruta : '/noticias',
      icon : 'flash'
    }
  ];

  itemsCuenta = [
    {
      label: 'Iniciar sesión',
      ruta: '/login',
      icon: 'log-in'
    },
    {
      label: 'Registrarse',
      ruta: '/registro',
      icon: 'disc'
    },
  ];

  auth = false;

  constructor(
    public authService: AuthService,
    private toastCtrl: ToastController
  ) { }

  async ngOnInit() {
    this.auth = await this.authService.isAuthenticated();
    // observable desde authService
    this.authService.loginState$
                    .subscribe((res: boolean) => this.auth = res);
  }

  async logout() {
    const toast = await this.toastCtrl.create({
      message: 'Sesión cerrada',
      duration: 2000
    });
    this.authService.logout();
    await toast.present();
  }

}
