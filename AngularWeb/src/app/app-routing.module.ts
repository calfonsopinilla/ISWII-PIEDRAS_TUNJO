import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import  {TableroComponent} from './componentes/tablero/tablero.component';
import  {LoginComponent} from './componentes/login/login.component';
import  {ConfiguracionComponent} from './componentes/configuracion/configuracion.component';
import  {GaleriaComponent} from './componentes/galeria/galeria.component';
import  {ContactanosComponent} from './componentes/contactanos/contactanos.component';
const routes: Routes = [
{path:'',component:TableroComponent},
{path:'galeria',component:GaleriaComponent},
{path:'login',component:LoginComponent},
{path:'configuracion',component:ConfiguracionComponent},
{path:'contactanos',component:ContactanosComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
 