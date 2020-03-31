import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canLoad(): Observable<boolean> | Promise<boolean> | boolean {
    this.authService.isAuthenticated()
                    .then((res: boolean) => {
                      if (!res) {
                        // this.router.navigate(['/login'], { queryParams: {redirect: true} });
                        this.router.navigateByUrl('/login');
                      }
                    });
    return this.authService.isAuthenticated();
  }

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    this.authService.isAuthenticated()
    .then((res: boolean) => {
      if (!res) {
        // this.router.navigate(['/login'], { queryParams: {redirect: true} });
        this.router.navigateByUrl('/login');
      }
    });
    return this.authService.isAuthenticated();
  }
}
