import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CabeceroComponent } from './componentes/cabecero/cabecero.component';
import { LoginComponent } from './componentes/login/login.component';
import { TableroComponent } from './componentes/tablero/tablero.component';
import { PiePaginaComponent } from './componentes/pie-pagina/pie-pagina.component';
import { ConfiguracionComponent } from './componentes/configuracion/configuracion.component';
import { ContactanosComponent } from './componentes/contactanos/contactanos.component';
import { GaleriaComponent } from './componentes/galeria/galeria.component';
import { SeccionInformativaComponent } from './componentes/seccion-informativa/seccion-informativa.component';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { CalendarModule } from '@syncfusion/ej2-angular-calendars';

@NgModule({
  declarations: [
    AppComponent,
    CabeceroComponent,
    LoginComponent,
    TableroComponent,
    PiePaginaComponent,
    ConfiguracionComponent,
    ContactanosComponent,
    GaleriaComponent,
    SeccionInformativaComponent,
    EventosComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CalendarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
