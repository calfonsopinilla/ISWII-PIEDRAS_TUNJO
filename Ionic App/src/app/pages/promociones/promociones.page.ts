import { Component, OnInit } from '@angular/core';
import { Promocion } from '../../interfaces/promocion'
import { PromocionesService } from '../../services/promociones.service';
import { AuthService } from '../../services/auth.service';
import { NavController, LoadingController, NavParams } from '@ionic/angular';
import { async } from '@angular/core/testing';
import { Router, NavigationExtras } from '@angular/router';
import { ModalController} from  '@ionic/angular'; 
import { ModalComentarioPage}  from '../../pages/noticias/modal-comentario/modal-comentario.page';

@Component({
  selector: 'app-promociones',
  templateUrl: './promociones.page.html',
  styleUrls: ['./promociones.page.scss'],
})
export class PromocionesPage implements OnInit {

  constructor(public navCtrl: NavController,
    private PromocionesServices: PromocionesService,
    public authService: AuthService,
    private loadingCtrl: LoadingController,
    private router: Router,
    //implementar al otro lado
    private modalCtrlr : ModalController
    ) { }
    
    promociones : Promocion [] = [];
    promocionSelect : Promocion;
    session : boolean;

    async  ngOnInit(){
      this.session = await this.authService.isAuthenticated();
      this.PromocionesServices.obtenerInformacionPromociones().subscribe(resp => {this. promociones = resp
      }); 
    }
    async adquirirPromocion(id){
      const loading = await this.loadingCtrl.create({ message: 'Espere por favor...' });
      await loading.present();
         if(this.session == true){
          this.promocionSelect = this.promociones[id];
          let navigationPromocion : NavigationExtras = {
              queryParams : {
                special : JSON.stringify(this.promocionSelect)
              }
          }
          this.router.navigate(['adquirir-promocion'],navigationPromocion);
          //this.navCtrl.navigateRoot('/adquirir-promocion');
        }else{
          this.navCtrl.navigateForward('/login');
          //dirigirlo al login 
        }
        await loading.dismiss();
    }

    async abrirOpciones(){
      const modal = await this.modalCtrlr.create({ 
          component: ModalComentarioPage,

          cssClass: 'my-custom-modal-css',
          componentProps :{
            vacio : 'vacio'
          }
        });
        await modal.present();
    }

}
