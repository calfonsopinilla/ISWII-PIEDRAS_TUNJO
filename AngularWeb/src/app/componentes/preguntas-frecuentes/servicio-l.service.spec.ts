import { TestBed } from '@angular/core/testing';

import { ServicioLService } from './servicio-l.service';

describe('ServicioLService', () => {
  let service: ServicioLService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioLService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
