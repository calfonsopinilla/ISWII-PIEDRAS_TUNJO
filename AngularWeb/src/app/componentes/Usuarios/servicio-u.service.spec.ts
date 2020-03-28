import { TestBed } from '@angular/core/testing';

import { ServicioUService } from './servicio-u.service';

describe('ServicioUService', () => {
  let service: ServicioUService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioUService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
