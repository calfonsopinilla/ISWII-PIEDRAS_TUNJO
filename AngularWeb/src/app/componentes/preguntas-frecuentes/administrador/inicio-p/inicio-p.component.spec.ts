import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InicioPComponent } from './inicio-p.component';

describe('InicioPComponent', () => {
  let component: InicioPComponent;
  let fixture: ComponentFixture<InicioPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InicioPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InicioPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
