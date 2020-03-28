import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InicioUComponent } from './inicio-u.component';

describe('InicioUComponent', () => {
  let component: InicioUComponent;
  let fixture: ComponentFixture<InicioUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InicioUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InicioUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
