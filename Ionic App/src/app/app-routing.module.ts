import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { VigilateGuard } from './guards/vigilante.guard';
import { VerifiedAccountGuard } from './guards/verified-account.guard';

const routes: Routes = [
  {
    path: 'inicio',
    canActivate: [ VigilateGuard ],
    loadChildren: () => import('./pages/inicio/inicio.module').then( m => m.InicioPageModule)
  },
  {
    path: 'page-not-found',
    loadChildren: () => import('./pages/page-not-found/page-not-found.module').then( m => m.PageNotFoundPageModule)
  },
  {
    path: 'descripcion-parque/:id',
    loadChildren: () => import('./pages/descripcion-parque/descripcion-parque.module').then( m => m.DescripcionParquePageModule)
  },
  {
    path: 'resenia-historica/:id',
    loadChildren: () => import('./pages/resenia-historica/resenia-historica.module').then( m => m.ReseniaHistoricaPageModule)
  },
  {
    path: 'ubicacion-parque/:id',
    loadChildren: () => import('./pages/ubicacion-parque/ubicacion-parque.module').then( m => m.UbicacionParquePageModule)
  },
  {
    path: 'piedras-parque/:id',
    loadChildren: () => import('./pages/piedras-parque/piedras-parque.module').then( m => m.PiedrasParquePageModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./pages/login/login.module').then( m => m.LoginPageModule)
  },
  {
    path: 'registro',
    loadChildren: () => import('./pages/registro/tabs/tabs.module').then( m => m.TabsPageModule)
  },
  {
    path: 'tickets',
    loadChildren: () => import('./pages/tickets/tabs/tabs.module').then( m => m.TabsPageModule),
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ]
  },
  {
    path: 'pqr-parque',
    loadChildren: () => import('./pages/pqr-parque/pqr-parque.module').then( m => m.PqrParquePageModule),
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ]
  },
  {
    path: 'preguntas-frecuentes',
    loadChildren: () => import('./pages/preguntas-frecuentes/preguntas-frecuentes.module').then( m => m.PreguntasFrecuentesPageModule),
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ]
  },
  {
    path: 'cuenta',
    loadChildren: () => import('./pages/cuenta/cuenta.module').then( m => m.CuentaPageModule),
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ]
  },
  {
    path: 'suscripciones',
    loadChildren: () => import('./pages/suscripciones/suscripciones.module').then( m => m.SuscripcionesPageModule),
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ]
  },
  {
    path: 'vigilante',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/vigilante/tabs/tabs.module').then( m => m.TabsPageModule)
  },
  {
    path: 'noticias',
    loadChildren: () => import('./pages/noticias/noticias.module').then( m => m.NoticiasPageModule)
  },
  {
    path: 'ver-noticia',
    loadChildren: () => import('./pages/ver-noticia/ver-noticia.module').then( m => m.VerNoticiaPageModule)
  },
  {
    path: 'cabanas',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/cabanas/tabs/tabs.module').then( m => m.TabsPageModule)
  },
  {
    path: 'promociones',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/promociones/tabs/tabs.module').then( m => m.TabsPageModule)
  },
  {
    path: 'recorridos',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/recorridos/recorridos.module').then( m => m.RecorridosPageModule)
  },
  {
    path: 'detalles-recorrido/:id',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/detalles-recorrido/detalles-recorrido.module').then( m => m.DetallesRecorridoPageModule)
  },
  {
    path: 'pictogramas',
    canLoad: [ VerifiedAccountGuard, AuthGuard ],
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./pages/pictogramas/pictogramas.module').then( m => m.PictogramasPageModule)
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
