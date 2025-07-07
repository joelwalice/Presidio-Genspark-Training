import { TestBed } from '@angular/core/testing';

import { RecruiterRegisterService } from './recruiter-register-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RecruiterRegisterService', () => {
  let service: RecruiterRegisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule],
      providers: [RecruiterRegisterService]
    });
    service = TestBed.inject(RecruiterRegisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
