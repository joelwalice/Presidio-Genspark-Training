import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecruiterApplicants } from './recruiter-applicants';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { JobSeekerService } from '../../services/user/job-seeker';

describe('Recruiter Applicants', () => {
  let component: RecruiterApplicants;
  let fixture: ComponentFixture<RecruiterApplicants>;
  let recruiterServiceSpy: jasmine.SpyObj<RecruiterService>;
  let jobSeekerServiceSpy: jasmine.SpyObj<JobSeekerService>;

  beforeEach(async () => {
    recruiterServiceSpy = jasmine.createSpyObj('RecruiterService', ['getJobApplicationsByJobId', 'updateApplicationStatus']);
    jobSeekerServiceSpy = jasmine.createSpyObj('JobSeekerService', ['getJobSeekerById', 'getResumeById']);
    recruiterServiceSpy.getJobApplicationsByJobId.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RecruiterApplicants],
      providers: [
        { provide: RecruiterService, useValue: recruiterServiceSpy },
        { provide: JobSeekerService, useValue: jobSeekerServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ jobId: '42' }),
            snapshot: { paramMap: { get: () => '42' } }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruiterApplicants);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the RecruiterApplicants component', () => {
    expect(component).toBeTruthy();
  });

  it('should receive jobId from route', () => {
    expect(component.jobId).toBe('42');
  });

  it('should open and close applicant details modal', () => {
    const applicant = { resumeContent: btoa('test'), resumeFileType: 'application/pdf' };
    component.openApplicantResume(applicant);
    expect(component.showModal).toBeTrue();
    expect(component.selectedApplicant).toBe(applicant);
    component.closeApplicantDetails();
    expect(component.showModal).toBeFalse();
    expect(component.selectedApplicant).toBeNull();
    expect(component.resumePreviewUrl).toBeUndefined();
  });

  it('should call recruiterService.updateApplicationStatus on updateStatus', () => {
    recruiterServiceSpy.updateApplicationStatus.and.returnValue(of({}));
    const applicant = { jobSeekerId: '1', jobStatus: 1 };
    component.updateStatus(applicant);
    expect(recruiterServiceSpy.updateApplicationStatus).toHaveBeenCalledWith('1', 1);
  });

  it('should call recruiterService.getJobApplicationsByJobId in fetchApplicants', () => {
    recruiterServiceSpy.getJobApplicationsByJobId.and.returnValue(of([]));
    component.fetchApplicants('42');
    expect(recruiterServiceSpy.getJobApplicationsByJobId).toHaveBeenCalledWith('42');
  });

  it('should set selectedApplicant and showModal in viewDetails', () => {
    const applicant = { id: 1 };
    component.viewDetails(applicant);
    expect(component.selectedApplicant).toBe(applicant);
    expect(component.showModal).toBeTrue();
  });

  it('should close modal in closeModal', () => {
    component.showModal = true;
    component.selectedApplicant = { id: 1 };
    component.closeModal();
    expect(component.showModal).toBeFalse();
    expect(component.selectedApplicant).toBeNull();
  });
});
