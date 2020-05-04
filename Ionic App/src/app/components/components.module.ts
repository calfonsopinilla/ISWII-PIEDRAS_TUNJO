import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { ItemInfoComponent } from './item-info/item-info.component';
import { PipesModule } from '../pipes/pipes.module';
import { AvatarSelectorComponent } from './avatar-selector/avatar-selector.component';
import { CabanaSelectorComponent } from './cabana-selector/cabana-selector.component';
import { CardRecorridoComponent } from './card-recorrido/card-recorrido.component';
import { CardPictogramaComponent } from './card-pictograma/card-pictograma.component';

@NgModule({
  declarations: [
    MenuComponent,
    HeaderComponent,
    ItemInfoComponent,
    AvatarSelectorComponent,
    CabanaSelectorComponent,
    CardRecorridoComponent,
    CardPictogramaComponent
  ],
  imports: [
    CommonModule,
    IonicModule,
    RouterModule,
    PipesModule
  ],
  exports: [
    MenuComponent,
    HeaderComponent,
    ItemInfoComponent,
    AvatarSelectorComponent,
    CabanaSelectorComponent,
    CardRecorridoComponent,
    CardPictogramaComponent
  ]
})
export class ComponentsModule { }
