import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { TransferirTicketPage } from './transferir-ticket.page';

describe('TransferirTicketPage', () => {
  let component: TransferirTicketPage;
  let fixture: ComponentFixture<TransferirTicketPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferirTicketPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(TransferirTicketPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
