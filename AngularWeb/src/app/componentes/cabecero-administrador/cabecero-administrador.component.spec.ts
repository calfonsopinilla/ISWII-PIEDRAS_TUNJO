import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CabeceroAdministradorComponent } from './cabecero-administrador.component';

describe('CabeceroAdministradorComponent', () => {
  let component: CabeceroAdministradorComponent;
  let fixture: ComponentFixture<CabeceroAdministradorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CabeceroAdministradorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CabeceroAdministradorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
