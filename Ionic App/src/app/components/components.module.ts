import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { ItemInfoComponent } from './item-info/item-info.component';
import { PipesModule } from '../pipes/pipes.module';

@NgModule({
  declarations: [
    MenuComponent,
    HeaderComponent,
    ItemInfoComponent
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
    ItemInfoComponent
  ]
})
export class ComponentsModule { }
