import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { SuscripcionesPage } from './suscripciones.page';

describe('SuscripcionesPage', () => {
  let component: SuscripcionesPage;
  let fixture: ComponentFixture<SuscripcionesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuscripcionesPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(SuscripcionesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
