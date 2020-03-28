import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarpiComponent } from './agregarpi.component';

describe('AgregarpiComponent', () => {
  let component: AgregarpiComponent;
  let fixture: ComponentFixture<AgregarpiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarpiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarpiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
