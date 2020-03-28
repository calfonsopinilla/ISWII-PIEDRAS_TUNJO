import { TestBed } from '@angular/core/testing';

import { ServiciocService } from './servicioc.service';

describe('ServiciocService', () => {
  let service: ServiciocService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServiciocService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
