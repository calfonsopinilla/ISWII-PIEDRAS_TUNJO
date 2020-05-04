import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AnonymousGuard implements CanActivate, CanLoad {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise(async resolve => {
      const auth = await this.authService.validateToken();
      if (auth) {
        this.router.navigateByUrl('/inicio');
      }
      resolve(!auth); // si está autentificado no deja ingresar al login ni registro, pero lo redirige a inicio
    });
  }
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
      return new Promise(async resolve => {
        const auth = await this.authService.validateToken();
        if (auth) {
          this.router.navigateByUrl('/inicio');
        }
        resolve(!auth); // si está autentificado no deja ingresar al login ni registro, pero lo redirige a inicio
      });
  }
}
