import { Directive, HostListener, ElementRef, Renderer } from '@angular/core';

@Directive({
  selector: '[appExpireDate]'
})
export class ExpireDateDirective {

  constructor(
    private el: ElementRef,
    private renderer: Renderer
  ) { }

  @HostListener('keypress', ['$event'])
  onKeypress(e: any) {
    if (isNaN(e.key)) {
      e.preventDefault();
    }
    if (this.value.length === 5) {
      e.preventDefault();
    }
  }

  @HostListener('keyup', ['$event'])
  onKeyup(e: any) {
    // si ya hay dos caracteres, ponemos el /
    if (this.value.length === 2 && !this.value.includes('/')) {
      this.setValue(this.value + '/');
    }

    // si hay 5 caracteres pero no está el /
    if (this.value.length === 5 && !this.value.includes('/')) {
      const replace = `${this.value.substr(0, 1)}/${this.value.substr(3, 4)}`;
      this.setValue(replace);
    }
    // cuando ya hay 5 caracteres incluyendo el / validamos el mes y año
    if (this.value.length === 5 && this.value.includes('/')) {
      const month = this.value.split('/')[0];
      const year = '20' + this.value.split('/')[1];
      this.setValue( this.validateMonth(month) + '/' + this.validateYear(year) );
    }
  }

  validateMonth(month: string) {
    if (Number(month) > 12) {
      return '12';
    } else if (Number(month) === 0) {
      return '01';
    }
    return month;
  }

  validateYear(year: string) {
    const currentYear = new Date().getFullYear();
    if (Number(year) < currentYear) {
      return currentYear.toString().substr(2, 3);
    }
    return year.substr(2, 3);
  }

  @HostListener('keyup.backspace', ['$event'])
  onkeyBackspace(e: any) {
    // si hay dos caracteres y uno es /
    if (this.value.length === 2 && this.value.includes('/')) {
      this.setValue(this.value[0]);
    }
    // si el primer caracter que queda es /
    if (this.value[0] === '/') {
      this.setValue(this.value.slice(1));
    }
    // si / queda como segundo caracter
    if (this.value[1] === '/') {
      this.setValue('0' + this.value);
    }
  }

  get value() {
    return this.el.nativeElement.value;
  }

  setValue(val: string) {
    this.renderer.setElementAttribute(this.el.nativeElement, 'value', val);
  }

}
