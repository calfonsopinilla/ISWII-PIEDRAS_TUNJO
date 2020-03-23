import { TestBed } from '@angular/core/testing';

import { ServiciologinService } from './serviciologin.service';

describe('ServiciologinService', () => {
  let service: ServiciologinService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServiciologinService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
