import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';
import { IonicModule } from '@ionic/angular';
import { VerNoticiasPageRoutingModule } from './ver-noticias-routing.module';
import { VerNoticiasPage } from './ver-noticias.page';
import { PipesModule } from '../../../pipes/pipes.module';
import { IonicRatingModule } from 'ionic4-rating';
@NgModule({
  imports: [  
    CommonModule,
    FormsModule,
    IonicModule,
    ReactiveFormsModule,
    VerNoticiasPageRoutingModule,
    ComponentsModule,
    IonicRatingModule, 
    PipesModule
  ],
  declarations: [VerNoticiasPage]
})
export class VerNoticiasPageModule {}
