import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import  {TableroComponent} from './componentes/tablero/tablero.component';
import  {LoginComponent} from './componentes/login/login.component';
import  {ConfiguracionComponent} from './componentes/configuracion/configuracion.component';
import  {GaleriaComponent} from './componentes/galeria/galeria.component';
import  {EditarClienteComponent} from './componentes/editar-cliente/editar-cliente.component';
import  {AgregarUsuarioComponent} from './componentes/agregar-usuario/agregar-usuario.component';
import  {ContactanosComponent} from './componentes/contactanos/contactanos.component';
import { ModuloQrComponent } from './componentes/modulo-qr/modulo-qr.component';
import { ModuloCComponent } from './componentes/modulo-c/modulo-c.component';
import { EliminarUsuarioComponent } from './componentes/eliminar-usuario/eliminar-usuario.component';
import { PreguntasFrecuentesComponent } from './componentes/preguntas-frecuentes/preguntas-frecuentes.component';
import  {InicioAdministradorComponent} from './componentes/inicio-administrador/inicio-administrador.component';
<<<<<<< HEAD
import { InicioAComponent } from './componentes/eventos/administrador/inicio-a/inicio-a.component';
import { AgregarComponent } from './componentes/eventos/administrador/agregar/agregar.component';
import { EditarComponent } from './componentes/eventos/administrador/editar/editar.component';
import { EliminarComponent } from './componentes/eventos/administrador/eliminar/eliminar.component';

=======
import { PictogramasComponent } from './componentes/pictogramas/pictogramas.component';
import { SubscripcionesComponent } from './componentes/subscripciones/subscripciones.component';
>>>>>>> be5cd15fbb092c4b8663914997a7b5e188c12281
const routes: Routes = [
{path:'',component:TableroComponent},
{path:'galeria',component:GaleriaComponent},
{path:'login',component:LoginComponent},
{path:'configuracion',component:ConfiguracionComponent},
{path:'contactanos',component:ContactanosComponent},
{path:'inicioadministrador',component:InicioAdministradorComponent},
{path:'Editar',component:EditarClienteComponent},
{path:'Agregar',component:AgregarUsuarioComponent},
{path:'Modulocabaña',component:ModuloCComponent},
{path:'ModuloQr',component:ModuloQrComponent},
{path:'ModuloQr',component:ModuloQrComponent},
<<<<<<< HEAD
{path:'eliminar',component:EliminarUsuarioComponent},
{path:'preguntasfrecuentes',component:PreguntasFrecuentesComponent},
{path:'inicioeventos',component:InicioAComponent},
{path:'agregarevento',component:AgregarComponent},
{path:'editarevento',component:EditarComponent},
{path:'eliminarevento',component:EliminarComponent},

=======
{path:'componentex',component:ComponentexComponent},
{path:'pictogramas',component:PictogramasComponent},
{path:'subscripciones',component:SubscripcionesComponent},
>>>>>>> be5cd15fbb092c4b8663914997a7b5e188c12281
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
 