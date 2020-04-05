import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ValidarNumeroDocumentoPage } from './validar-numero-documento.page';

describe('ValidarNumeroDocumentoPage', () => {
  let component: ValidarNumeroDocumentoPage;
  let fixture: ComponentFixture<ValidarNumeroDocumentoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarNumeroDocumentoPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ValidarNumeroDocumentoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
