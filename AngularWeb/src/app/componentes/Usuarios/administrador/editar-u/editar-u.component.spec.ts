import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarUComponent } from './editar-u.component';

describe('EditarUComponent', () => {
  let component: EditarUComponent;
  let fixture: ComponentFixture<EditarUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
