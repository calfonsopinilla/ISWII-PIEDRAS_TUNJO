import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { FotoDocumentoPage } from './foto-documento.page';

describe('FotoDocumentoPage', () => {
  let component: FotoDocumentoPage;
  let fixture: ComponentFixture<FotoDocumentoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FotoDocumentoPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(FotoDocumentoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
