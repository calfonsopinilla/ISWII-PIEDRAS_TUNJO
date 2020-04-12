import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { PromocionesPageRoutingModule } from './promociones-routing.module';
import { PromocionesPage } from './promociones.page';
import {  ComponentsModule } from '../../components/components.module';
import { PipesModule } from '../../pipes/pipes.module';
import { ModalComentarioPage} from '../../pages/noticias/modal-comentario/modal-comentario.page';
import {ModalComentarioPageModule} from '../../pages/noticias/modal-comentario/modal-comentario.module';  

@NgModule({
  entryComponents:[
    ModalComentarioPage

  ],


  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PromocionesPageRoutingModule,
    PipesModule,
    ComponentsModule,
    ModalComentarioPageModule
    
  ],
  declarations: [PromocionesPage]
})
export class PromocionesPageModule {}
