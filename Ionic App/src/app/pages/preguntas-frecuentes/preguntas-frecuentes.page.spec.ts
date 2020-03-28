import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { PreguntasFrecuentesPage } from './preguntas-frecuentes.page';

describe('PreguntasFrecuentesPage', () => {
  let component: PreguntasFrecuentesPage;
  let fixture: ComponentFixture<PreguntasFrecuentesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreguntasFrecuentesPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(PreguntasFrecuentesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
