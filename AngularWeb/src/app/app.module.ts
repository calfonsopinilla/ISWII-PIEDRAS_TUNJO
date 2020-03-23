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
import { EditarClienteComponent } from './componentes/editar-cliente/editar-cliente.component';
import { AgregarUsuarioComponent } from './componentes/agregar-usuario/agregar-usuario.component';
import { CabeceroAdministradorComponent } from './componentes/cabecero-administrador/cabecero-administrador.component';
import { ModuloQrComponent } from './componentes/modulo-qr/modulo-qr.component';
import { ModuloCComponent } from './componentes/modulo-c/modulo-c.component';

import { ServicioEventoService } from './componentes/eventos/servicio-evento.service';
import { ServicioAdminService } from './componentes/inicio-administrador/servicio-admin.service';
import { ServicioInsertService } from './componentes/agregar-usuario/servicio-insert.service';
import { ServicioInfoService } from './componentes/seccion-informativa/servicio-info.service';
import { ServiciologinService} from './componentes/login/serviciologin.service';
import { ServicioEditarService } from './componentes/editar-cliente/servicio-editar.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { EliminarUsuarioComponent } from './componentes/eliminar-usuario/eliminar-usuario.component';
import { ServicioeliminarUService } from './componentes/eliminar-usuario/servicioeliminar-u.service';
import { PreguntasFrecuentesComponent } from './componentes/preguntas-frecuentes/preguntas-frecuentes.component';
import { InicioAComponent } from './componentes/eventos/administrador/inicio-a/inicio-a.component';
import { ServiciomostrareService } from './componentes/eventos/administrador/inicio-a/serviciomostrare.service';
import { AgregarComponent } from './componentes/eventos/administrador/agregar/agregar.component';
import { ServicioagregareService } from './componentes/eventos/administrador/agregar/servicioagregare.service';
import { EditarComponent } from './componentes/eventos/administrador/editar/editar.component';
import { ServicioeditareService } from './componentes/eventos/administrador/editar/servicioeditare.service';
import { EliminarComponent } from './componentes/eventos/administrador/eliminar/eliminar.component';
import { ServicioeliminareService } from './componentes/eventos/administrador/eliminar/servicioeliminare.service';


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
    EditarClienteComponent,
    AgregarUsuarioComponent,
    CabeceroAdministradorComponent,
    ModuloQrComponent,
    ModuloCComponent,
    EliminarUsuarioComponent,
    PreguntasFrecuentesComponent,
    InicioAComponent,
    AgregarComponent,
    EditarComponent,
    EliminarComponent,
  
   


     //ServicioEventoService
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
  ServicioInsertService,ServicioAdminService,ServiciologinService
  ,ServicioEditarService,ServicioeliminarUService,ServiciomostrareService
  ,ServicioagregareService , ServicioeditareService,ServicioeliminareService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
