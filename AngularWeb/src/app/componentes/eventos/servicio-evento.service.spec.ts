import { TestBed } from '@angular/core/testing';

import { ServicioEventoService } from './servicio-evento.service';

describe('ServicioEventoService', () => {
  let service: ServicioEventoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioEventoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
