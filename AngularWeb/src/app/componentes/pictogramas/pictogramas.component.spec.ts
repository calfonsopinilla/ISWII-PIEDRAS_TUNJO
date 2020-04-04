import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PictogramasComponent } from './pictogramas.component';

describe('PictogramasComponent', () => {
  let component: PictogramasComponent;
  let fixture: ComponentFixture<PictogramasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PictogramasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PictogramasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
