import { TestBed } from '@angular/core/testing';

import { ServicioeliminareService } from './servicioeliminare.service';

describe('ServicioeliminareService', () => {
  let service: ServicioeliminareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioeliminareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
