import { TestBed } from '@angular/core/testing';

import { RecruiterRegisterService } from './recruiter-register-service';

describe('RecruiterRegisterService', () => {
  let service: RecruiterRegisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruiterRegisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
