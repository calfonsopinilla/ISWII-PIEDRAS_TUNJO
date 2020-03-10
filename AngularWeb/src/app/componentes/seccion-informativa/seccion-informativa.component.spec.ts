import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeccionInformativaComponent } from './seccion-informativa.component';

describe('SeccionInformativaComponent', () => {
  let component: SeccionInformativaComponent;
  let fixture: ComponentFixture<SeccionInformativaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeccionInformativaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeccionInformativaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
