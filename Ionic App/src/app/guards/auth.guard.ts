import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { LoadingController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanLoad {

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
}
