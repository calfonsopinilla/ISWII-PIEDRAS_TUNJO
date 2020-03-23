import { TestBed } from '@angular/core/testing';

import { ServicioeliminarUService } from './servicioeliminar-u.service';

describe('ServicioeliminarUService', () => {
  let service: ServicioeliminarUService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioeliminarUService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
