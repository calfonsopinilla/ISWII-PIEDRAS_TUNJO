import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class VigilateGuard implements CanLoad, CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canLoad(
    route: Route,
    segments: UrlSegment[]
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.esVigilante();
  }

  canActivate(): Promise<boolean> {
   return new Promise(async resolve => {
     const auth = await this.authService.validateToken();
     if (auth) {
        const esVigilante = await this.authService.esVigilante(false); // no redirectTo login
        if (esVigilante) { // si es vigilante
          this.router.navigateByUrl('/vigilante');
        }
        // opuesto porque si es un vigilante no puede pasar al inicio porque se redirecciona a vigilante
        resolve(!esVigilante);
     } else {
       resolve(true); // si no está autenticado puede pasar sin problema
     }
   });
  }
}
