import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { IngresarCodigoPage } from './ingresar-codigo.page';

describe('IngresarCodigoPage', () => {
  let component: IngresarCodigoPage;
  let fixture: ComponentFixture<IngresarCodigoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IngresarCodigoPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(IngresarCodigoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
