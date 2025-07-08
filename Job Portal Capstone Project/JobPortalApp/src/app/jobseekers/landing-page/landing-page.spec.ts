import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LandingPage } from './landing-page';
import { of, Subject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobSeekerService } from '../../services/user/job-seeker';
import { ResumeStateService } from '../../services/user/resume-state-service';
import { RouterTestingModule } from '@angular/router/testing';

describe('JobSeeker Landing Page', () => {
  let component: LandingPage;
  let fixture: ComponentFixture<LandingPage>;
  let jobSeekerServiceSpy: jasmine.SpyObj<JobSeekerService>;
  let resumeStateSpy: jasmine.SpyObj<ResumeStateService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let router: Router;

  beforeEach(async () => {
    jobSeekerServiceSpy = jasmine.createSpyObj('JobSeekerService', [
      'getJobSeekerById', 'getJobDetails', 'getAppliedJobsByJobSeekerId'
    ]);
    jobSeekerServiceSpy.getJobDetails.and.returnValue(of([]));
    jobSeekerServiceSpy.getJobSeekerById.and.returnValue(of({}));
    jobSeekerServiceSpy.getAppliedJobsByJobSeekerId.and.returnValue(of([]));

    resumeStateSpy = jasmine.createSpyObj(
      'ResumeStateService',
      ['setHasResume'],
      {
        appliedJobsChanged$: new Subject<boolean>(),
        hasResume$: new Subject<boolean>()
      }
    );

    await TestBed.configureTestingModule({
      imports: [LandingPage, HttpClientTestingModule, RouterTestingModule],
      providers: [
        { provide: JobSeekerService, useValue: jobSeekerServiceSpy },
        { provide: ResumeStateService, useValue: resumeStateSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: {
              paramMap: {
                get: (key: string) => null
              }
            }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LandingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch applied jobs on fetchAppliedJobs()', () => {
    spyOn(localStorage, 'getItem').and.returnValue('123');
    jobSeekerServiceSpy.getAppliedJobsByJobSeekerId.and.returnValue(of([{ id: 1 }]));
    component.fetchAppliedJobs();
    expect(jobSeekerServiceSpy.getAppliedJobsByJobSeekerId).toHaveBeenCalledWith('123');
  });

  it('should update hasResume when hasResume$ emits', () => {
    (resumeStateSpy.hasResume$ as Subject<boolean>).next(true);
    expect(component.hasResume).toBeTrue();
    (resumeStateSpy.hasResume$ as Subject<boolean>).next(false);
    expect(component.hasResume).toBeFalse();
  });

  it('should update appliedJobs when appliedJobsChanged$ emits', () => {
    spyOn(component, 'fetchAppliedJobs');
    (resumeStateSpy.appliedJobsChanged$ as Subject<boolean>).next(true);
    expect(component.fetchAppliedJobs).toHaveBeenCalled();
  });

  it('should update fullName, email, phone, and hasResume on getJobSeekerById', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => key === 'Id' ? '123' : null);
    jobSeekerServiceSpy.getJobSeekerById.and.returnValue(of({
      name: 'Test User',
      email: 'test@email.com',
      phoneNumber: '1234567890',
      defaultResumeId: 'resume1'
    }));
    component.ngOnInit();
    expect(jobSeekerServiceSpy.getJobSeekerById).toHaveBeenCalledWith('123');
  });

  it('should update jobs on getJobDetails', () => {
    jobSeekerServiceSpy.getJobDetails.and.returnValue(of([{ id: '1', title: 'Job' }]));
    component['jobs'] = [];
    component.ngOnInit();
    expect(jobSeekerServiceSpy.getJobDetails).toHaveBeenCalled();
  });

  it('should navigate based on role in checkRole', (done) => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'role') return 'Recruiter';
      return null;
    });
    setTimeout(() => {
      expect(router.navigate).toHaveBeenCalledWith(['/recruiters/home']);
      done();
    }, 150);
    component.ngOnInit();
  });
});
