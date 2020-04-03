import { Directive, Injectable } from '@angular/core';
import { UserService } from '../services/user.service';
import { AsyncValidator, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })

export class EmailValidator implements AsyncValidator {
  constructor( private userService: UserService ) {}

  validate(
    ctrl: AbstractControl
  ): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return this.userService.existeCorreo(ctrl.value)
                    .pipe(
                      map(exists => (exists ? {emailExists: true} : null)),
                      catchError(_ => null)
                  );
  }

  validateToken(ctrl: AbstractControl) : Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return this.userService.existeCorreoToken(ctrl.value)
                            .pipe(
                              map(exists => (exists ? {emailTokenExists: true} : null)),
                              catchError(_ => null)
                            );
  }
}

@Directive({
  selector: '[appEmail]'
})
export class EmailDirective {

  constructor() { }

}
