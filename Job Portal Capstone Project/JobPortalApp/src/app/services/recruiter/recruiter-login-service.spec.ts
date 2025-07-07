import { TestBed } from '@angular/core/testing';

import { RecruiterLoginService } from './recruiter-login-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('RecruiterLoginService', () => {
  let service: RecruiterLoginService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule],
      providers: [RecruiterLoginService]
    });
    service = TestBed.inject(RecruiterLoginService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
