import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { DetallePictogramaPage } from './detalle-pictograma.page';

describe('DetallePictogramaPage', () => {
  let component: DetallePictogramaPage;
  let fixture: ComponentFixture<DetallePictogramaPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetallePictogramaPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(DetallePictogramaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
