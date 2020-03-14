import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuloQrComponent } from './modulo-qr.component';

describe('ModuloQrComponent', () => {
  let component: ModuloQrComponent;
  let fixture: ComponentFixture<ModuloQrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuloQrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuloQrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
