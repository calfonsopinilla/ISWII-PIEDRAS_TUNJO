import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarcComponent } from './agregarc.component';

describe('AgregarcComponent', () => {
  let component: AgregarcComponent;
  let fixture: ComponentFixture<AgregarcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
