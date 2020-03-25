import { TestBed } from '@angular/core/testing';

import { ServiciomostrareService } from './serviciomostrare.service';

describe('ServiciomostrareService', () => {
  let service: ServiciomostrareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServiciomostrareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
