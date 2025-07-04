import { TestBed } from '@angular/core/testing';

import { RecruiterLoginService } from './recruiter-login-service';

describe('RecruiterLoginService', () => {
  let service: RecruiterLoginService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruiterLoginService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
