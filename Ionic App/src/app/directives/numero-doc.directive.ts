import { Directive, Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors, AsyncValidator } from '@angular/forms';
import { Observable } from 'rxjs';
import { UserService } from '../services/user.service';
import { map, catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })

export class NumeroDocValidator implements AsyncValidator {
  constructor( private userService: UserService ) {}

  validate(
    ctrl: AbstractControl
  ): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return this.userService.existeNumeroDocumento(ctrl.value)
                    .pipe(
                      map(exists => (exists ? {numeroDocExists: true} : null)),
                      catchError(_ => null)
                  );
  }
}

@Directive({
  selector: '[appNumeroDoc]'
})
export class NumeroDocDirective {

  constructor() { }

}

