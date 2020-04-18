import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appMaxLength]'
})
export class MaxLengthDirective {

  @Input('appMaxLength') maxLength: string;

  constructor(
    private el: ElementRef
  ) { }

  @HostListener('keypress', ['$event'])
  onKeyPress(e: any) {
    if (isNaN(e.key)) {
      e.preventDefault();
    }
    if (this.value.length === Number(this.maxLength)) {
      e.preventDefault();
    }
  }

  get value() {
    return this.el.nativeElement.value;
  }

}
