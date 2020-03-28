import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 

import { ReactiveFormsModule} from '@angular/forms' 

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
import { InicioAdministradorComponent } from './componentes/inicio-administrador/inicio-administrador.component';

import { CabeceroAdministradorComponent } from './componentes/cabecero-administrador/cabecero-administrador.component';
import { ModuloQrComponent } from './componentes/modulo-qr/modulo-qr.component';

import { ServicioEventoService } from './componentes/eventos/servicio-evento.service';
import { ServicioAdminService } from './componentes/inicio-administrador/servicio-admin.service';

import { ServicioInfoService } from './componentes/seccion-informativa/servicio-info.service';
import { ServiciologinService} from './componentes/login/serviciologin.service';

import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';


import { PreguntasFrecuentesComponent } from './componentes/preguntas-frecuentes/preguntas-frecuentes.component';
import { InicioAComponent } from './componentes/eventos/administrador/inicio-a/inicio-a.component';

import { AgregarComponent } from './componentes/eventos/administrador/agregar/agregar.component';

import { EditarComponent } from './componentes/eventos/administrador/editar/editar.component';



import { ServicioLService } from './componentes/preguntas-frecuentes/servicio-l.service';


import { PictogramasComponent } from './componentes/pictogramas/pictogramas.component';
import { SubscripcionesComponent } from './componentes/subscripciones/subscripciones.component';
import { InicioPComponent } from './componentes/preguntas-frecuentes/administrador/inicio-p/inicio-p.component';
import { AgregarPComponent } from './componentes/preguntas-frecuentes/administrador/agregar-p/agregar-p.component';
import { EditarPComponent } from './componentes/preguntas-frecuentes/administrador/editar-p/editar-p.component';
import { IniciocComponent } from './componentes/cabana/administrador/inicioc/inicioc.component';
import { EditarcComponent } from './componentes/cabana/administrador/editarc/editarc.component';
import { AgregarcComponent } from './componentes/cabana/administrador/agregarc/agregarc.component';

import { ServiciocService } from './componentes/cabana/servicioc.service';
import { IniciopiComponent } from './componentes/puntosInteres/administrador/iniciopi/iniciopi.component';
import { EditarpiComponent } from './componentes/puntosInteres/administrador/editarpi/editarpi.component';
import { AgregarpiComponent } from './componentes/puntosInteres/administrador/agregarpi/agregarpi.component';
import { ServiciopiService } from './componentes/puntosInteres/serviciopi.service';
import { InicioUComponent } from './componentes/Usuarios/administrador/inicio-u/inicio-u.component';
import { EditarUComponent } from './componentes/Usuarios/administrador/editar-u/editar-u.component';
import { AgregarUComponent } from './componentes/Usuarios/administrador/agregar-u/agregar-u.component';
import { ServicioUService } from './componentes/Usuarios/servicio-u.service';
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
    EventosComponent,
    InicioAdministradorComponent,
    CabeceroAdministradorComponent,
    ModuloQrComponent,
    PreguntasFrecuentesComponent,
    InicioAComponent,
    AgregarComponent,
    EditarComponent,
    PictogramasComponent,
    SubscripcionesComponent,
    InicioPComponent,
    AgregarPComponent,
    EditarPComponent,
    IniciocComponent,
    EditarcComponent,
    AgregarcComponent,
    IniciopiComponent,
    EditarpiComponent,
    AgregarpiComponent,
    InicioUComponent,
    EditarUComponent,
    AgregarUComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
     FormsModule,
    CalendarModule,
    ReactiveFormsModule,
    HttpClientModule,
     HttpModule
  ],
  providers: [ ServicioEventoService , ServicioInfoService , 
  ServicioAdminService,ServiciologinService
 ,ServicioLService ,ServiciocService , ServiciopiService,
 ServicioUService],
  bootstrap: [AppComponent]
})
export class AppModule { }
