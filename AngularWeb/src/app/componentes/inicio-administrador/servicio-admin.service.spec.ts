import { TestBed } from '@angular/core/testing';

import { ServicioAdminService } from './servicio-admin.service';

describe('ServicioAdminService', () => {
  let service: ServicioAdminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioAdminService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
