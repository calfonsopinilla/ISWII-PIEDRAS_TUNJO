import { TestBed } from '@angular/core/testing';

import { ServiciopiService  } from './serviciopi.service';

describe('ServiciopiService ', () => {
  let service: ServiciopiService ;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServiciopiService );
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
