import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ApplyJobs } from './apply-jobs';
import { JobSeekerService } from '../../services/user/job-seeker';
import { ResumeStateService } from '../../services/user/resume-state-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('JobSeeker Apply Jobs', () => {
  let component: ApplyJobs;
  let fixture: ComponentFixture<ApplyJobs>;
  let jobSeekerServiceSpy: jasmine.SpyObj<JobSeekerService>;
  let resumeStateSpy: jasmine.SpyObj<ResumeStateService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    jobSeekerServiceSpy = jasmine.createSpyObj('JobSeekerService', [
      'getJobDetailById',
      'getResumesByJobseekerId',
      'applyJobs'
    ]);
    jobSeekerServiceSpy.getJobDetailById.and.returnValue(of({}));
    jobSeekerServiceSpy.getResumesByJobseekerId.and.returnValue(of([]));
    resumeStateSpy = jasmine.createSpyObj('ResumeStateService', ['notifyAppliedJobChange']);
    routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [ApplyJobs, HttpClientTestingModule],
      providers: [
        { provide: JobSeekerService, useValue: jobSeekerServiceSpy },
        { provide: ResumeStateService, useValue: resumeStateSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: '42' }),
            snapshot: { paramMap: { get: () => '42' } }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ApplyJobs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });


  it('should not apply if no resume is selected', () => {
    spyOn(window, 'alert');
    component.selectedResumeId = '';
    component.applyToJob();
    expect(window.alert).toHaveBeenCalledWith('Please select a resume.');
  });

  it('should call applyJobs and navigate on success', () => {
    component.selectedResumeId = 'res123';
    component.jobId = '42';
    component.jobSeekerId = 'user123';
    jobSeekerServiceSpy.applyJobs.and.returnValue(of({ message: 'Applied successfully' }));
    component.applyToJob();
    expect(jobSeekerServiceSpy.applyJobs).toHaveBeenCalledWith({
      jobId: '42',
      jobSeekerId: 'user123',
      resumeDocumentId: 'res123'
    });
    expect(resumeStateSpy.notifyAppliedJobChange).toHaveBeenCalled();
    setTimeout(() => {
      expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/jobseekers');
    }, 2000);
  });

  it('should set error message on apply failure', () => {
    component.selectedResumeId = 'res123';
    component.jobId = '42';
    component.jobSeekerId = 'user123';
    jobSeekerServiceSpy.applyJobs.and.returnValue(throwError(() => ({ error: { error: 'Failed to apply' } })));
    component.applyToJob();
    expect(component.message).toBe('Failed to apply');
  });
});
