import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarPComponent } from './editar-p.component';

describe('EditarPComponent', () => {
  let component: EditarPComponent;
  let fixture: ComponentFixture<EditarPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
