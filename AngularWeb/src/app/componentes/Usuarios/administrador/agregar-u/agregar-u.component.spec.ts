import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarUComponent } from './agregar-u.component';

describe('AgregarUComponent', () => {
  let component: AgregarUComponent;
  let fixture: ComponentFixture<AgregarUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
