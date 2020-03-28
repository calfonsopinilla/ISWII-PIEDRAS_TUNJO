import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarpiComponent } from './editarpi.component';

describe('EditarpiComponent', () => {
  let component: EditarpiComponent;
  let fixture: ComponentFixture<EditarpiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarpiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarpiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
