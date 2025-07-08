import { TestBed } from '@angular/core/testing';

import { ResumeStateService } from './resume-state-service';

describe('JobSeeker Resume State Service', () => {
  let service: ResumeStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResumeStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
