import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'inicio',
    loadChildren: () => import('./pages/inicio/inicio.module').then( m => m.InicioPageModule)
  },
  {
    path: 'page-not-found',
    loadChildren: () => import('./pages/page-not-found/page-not-found.module').then( m => m.PageNotFoundPageModule)
  },
  {
    path: 'descripcion-parque',
    loadChildren: () => import('./pages/descripcion-parque/descripcion-parque.module').then( m => m.DescripcionParquePageModule)
  },
  {
    path: 'resenia-historica',
    loadChildren: () => import('./pages/resenia-historica/resenia-historica.module').then( m => m.ReseniaHistoricaPageModule)
  },
  {
    path: 'ubicacion-parque',
    loadChildren: () => import('./pages/ubicacion-parque/ubicacion-parque.module').then( m => m.UbicacionParquePageModule)
  },
  {
    path: 'piedras-parque',
    loadChildren: () => import('./pages/piedras-parque/piedras-parque.module').then( m => m.PiedrasParquePageModule)
  },
  { path: '', redirectTo: 'inicio', pathMatch: 'full' },
  {
    path: '**',
    loadChildren: () => import('./pages/page-not-found/page-not-found.module').then(m => m.PageNotFoundPageModule)
  }

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
