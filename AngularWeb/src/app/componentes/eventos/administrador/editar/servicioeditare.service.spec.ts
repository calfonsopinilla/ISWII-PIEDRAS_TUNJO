import { TestBed } from '@angular/core/testing';

import { ServicioeditareService } from './servicioeditare.service';

describe('ServicioeditareService', () => {
  let service: ServicioeditareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicioeditareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
