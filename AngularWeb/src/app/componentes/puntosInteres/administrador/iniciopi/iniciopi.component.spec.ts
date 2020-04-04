import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IniciopiComponent } from './iniciopi.component';

describe('IniciopiComponent', () => {
  let component: IniciopiComponent;
  let fixture: ComponentFixture<IniciopiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IniciopiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IniciopiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
