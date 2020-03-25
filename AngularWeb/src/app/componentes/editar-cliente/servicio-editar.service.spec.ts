import { TestBed } from '@angular/core/testing';

import { ServicioEditarService } from './servicio-editar.service';

describe('ServicioEditarService', () => {
  let service: ServicioEditarService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioEditarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
