import { TestBed } from '@angular/core/testing';

import { ServicioxService } from './serviciox.service';

describe('ServicioxService', () => {
  let service: ServicioxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
