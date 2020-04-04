import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { VerNoticiaPageRoutingModule } from './ver-noticia-routing.module';
import { VerNoticiaPage } from './ver-noticia.page';
import { PipesModule } from '../../pipes/pipes.module';
import { ComponentsModule } from '../../components/components.module';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VerNoticiaPageRoutingModule,PipesModule,
    ComponentsModule
  ],
  declarations: [VerNoticiaPage]
})
export class VerNoticiaPageModule {}
