import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {

  constructor(
    private authService: AuthService
  ) {}

  canLoad(): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.validateToken(true);
  }

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.validateToken(true);
  }
}
