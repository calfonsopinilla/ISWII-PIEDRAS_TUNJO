import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IniciocComponent } from './inicioc.component';

describe('IniciocComponent', () => {
  let component: IniciocComponent;
  let fixture: ComponentFixture<IniciocComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IniciocComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IniciocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
