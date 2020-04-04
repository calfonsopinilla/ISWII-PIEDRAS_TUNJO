import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarPComponent } from './agregar-p.component';

describe('AgregarPComponent', () => {
  let component: AgregarPComponent;
  let fixture: ComponentFixture<AgregarPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
