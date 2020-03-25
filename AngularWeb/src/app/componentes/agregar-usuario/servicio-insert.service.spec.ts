import { TestBed } from '@angular/core/testing';

import { ServicioInsertService } from './servicio-insert.service';

describe('ServicioInsertService', () => {
  let service: ServicioInsertService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioInsertService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
