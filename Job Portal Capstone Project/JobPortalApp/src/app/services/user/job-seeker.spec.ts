import { TestBed } from '@angular/core/testing';

import { JobSeekerService } from './job-seeker';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('JobSeeker Service', () => {
  let service: JobSeekerService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[ HttpClientTestingModule]
    });
    service = TestBed.inject(JobSeekerService);
  });

  it('JobSeeker Service should be created', () => {
    expect(service).toBeTruthy();
  });
});
