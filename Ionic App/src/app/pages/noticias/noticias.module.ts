import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { NoticiasPageRoutingModule } from './noticias-routing.module';
import { NoticiasPage } from './noticias.page';
import { PipesModule } from '../../pipes/pipes.module';
import { ComponentsModule } from '../../components/components.module';



@NgModule({
   
  
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NoticiasPageRoutingModule,
    PipesModule,
    ComponentsModule
  ],
  declarations: [NoticiasPage]
})
export class NoticiasPageModule {}
