import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ValidarQrPage } from './validar-qr.page';

describe('ValidarQrPage', () => {
  let component: ValidarQrPage;
  let fixture: ComponentFixture<ValidarQrPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarQrPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ValidarQrPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
