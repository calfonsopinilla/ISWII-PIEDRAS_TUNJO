import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import  {TableroComponent} from './componentes/tablero/tablero.component';
import  {LoginComponent} from './componentes/login/login.component';
import  {ConfiguracionComponent} from './componentes/configuracion/configuracion.component';
import  {GaleriaComponent} from './componentes/galeria/galeria.component';

import  {ContactanosComponent} from './componentes/contactanos/contactanos.component';
import { ModuloQrComponent } from './componentes/modulo-qr/modulo-qr.component';
import { PreguntasFrecuentesComponent } from './componentes/preguntas-frecuentes/preguntas-frecuentes.component';
import  {InicioAdministradorComponent} from './componentes/inicio-administrador/inicio-administrador.component';

import { InicioAComponent } from './componentes/eventos/administrador/inicio-a/inicio-a.component';
import { AgregarComponent } from './componentes/eventos/administrador/agregar/agregar.component';
import { EditarComponent } from './componentes/eventos/administrador/editar/editar.component';
import { InicioPComponent } from './componentes/preguntas-frecuentes/administrador/inicio-p/inicio-p.component';
import { AgregarPComponent } from './componentes/preguntas-frecuentes/administrador/agregar-p/agregar-p.component';
import { EditarPComponent } from './componentes/preguntas-frecuentes/administrador/editar-p/editar-p.component';

import { PictogramasComponent } from './componentes/pictogramas/pictogramas.component';
import { SubscripcionesComponent } from './componentes/subscripciones/subscripciones.component';

import { IniciocComponent } from './componentes/cabana/administrador/inicioc/inicioc.component';
import { EditarcComponent } from './componentes/cabana/administrador/editarc/editarc.component';
import { AgregarcComponent } from './componentes/cabana/administrador/agregarc/agregarc.component';

import { IniciopiComponent } from './componentes/puntosInteres/administrador/iniciopi/iniciopi.component';
import { EditarpiComponent } from './componentes/puntosInteres/administrador/editarpi/editarpi.component';
import { AgregarpiComponent } from './componentes/puntosInteres/administrador/agregarpi/agregarpi.component';

import { InicioUComponent } from './componentes/Usuarios/administrador/inicio-u/inicio-u.component';
import { EditarUComponent } from './componentes/Usuarios/administrador/editar-u/editar-u.component';
import { AgregarUComponent } from './componentes/Usuarios/administrador/agregar-u/agregar-u.component';
import { PuntosInteresComponent } from './componentes/puntos-interes/puntos-interes.component';

const routes: Routes = [

{path:'',component:TableroComponent},
{path:'galeria',component:GaleriaComponent},
{path:'login',component:LoginComponent},
{path:'configuracion',component:ConfiguracionComponent},
{path:'contactanos',component:ContactanosComponent},
{path:'inicioadministrador',component:InicioAdministradorComponent},
{path:'ModuloQr',component:ModuloQrComponent},
{path:'preguntasfrecuentes',component:PreguntasFrecuentesComponent},
{path:'inicioeventos',component:InicioAComponent},
{path:'agregarevento',component:AgregarComponent},
{path:'editarevento/:id',component:EditarComponent},
{path:'inicioaPf',component:InicioPComponent},
{path:'agregarpf',component:AgregarPComponent},
{path:'editarpf/:id',component:EditarPComponent},
{path:'pictogramas',component:PictogramasComponent},
{path:'subscripciones',component:SubscripcionesComponent},
{path:'inicioc',component:IniciocComponent},
{path:'agregarc',component:AgregarcComponent},
{path:'editarc/:id',component:EditarcComponent},
{path:'iniciopi',component:IniciopiComponent},
{path:'agregarpi',component:AgregarpiComponent},
{path:'editarpi/:id',component:EditarpiComponent},
{path:'iniciou',component:InicioUComponent},
{path:'agregaru',component:AgregarUComponent},
{path:'editaru/:id',component:EditarUComponent},
{path:'puntos-interes',component: PuntosInteresComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
 