import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuloCComponent } from './modulo-c.component';

describe('ModuloCComponent', () => {
  let component: ModuloCComponent;
  let fixture: ComponentFixture<ModuloCComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuloCComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuloCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
