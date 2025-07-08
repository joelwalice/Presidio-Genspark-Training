import { TestBed } from '@angular/core/testing';
import { JobSeekerService } from './job-seeker';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('JobSeeker Service', () => {
  let service: JobSeekerService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[ HttpClientTestingModule ]
    });
    service = TestBed.inject(JobSeekerService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
    localStorage.setItem('JwtToken', 'test-token');
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get job seeker by id', () => {
    service.getJobSeekerById('123').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobseeker/123');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should fetch all job seekers', () => {
    service.fetchAllJobSeeker().subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobseeker');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush([]);
  });

  it('should delete job seeker', () => {
    service.deleteJobSeeker('123').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobseeker/123');
    expect(req.request.method).toBe('DELETE');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should get resumes by jobseeker id', () => {
    service.getResumesByJobseekerId('abc').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobseeker/resumes/abc');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush([]);
  });

  it('should upload resume', () => {
    const formData = new FormData();
    service.uploadResume(formData).subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/files/upload');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should delete resume', () => {
    service.deleteResume('resume123').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/files/resume123');
    expect(req.request.method).toBe('DELETE');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should get job details', () => {
    service.getJobDetails().subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobs');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush([]);
  });

  it('should get job detail by id', () => {
    service.getJobDetailById('job1').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobs/job1');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should get resume by id', () => {
    service.getResumeById('resume1').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/files/resume/resume1');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should get applied jobs by job seeker id', () => {
    service.getAppliedJobsByJobSeekerId('js1').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/JobApplication/applied/js1');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should apply for jobs', () => {
    const payload = { jobId: 'job1' };
    service.applyJobs(payload).subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobs/apply');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should update profile', () => {
    const payload = { name: 'Test' };
    service.updateProfile(payload).subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/jobseeker');
    expect(req.request.method).toBe('PUT');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should set default resume', () => {
    service.setDefaultResume('js1', 'resume1').subscribe();
    const req = httpMock.expectOne('http://localhost:5039/api/files/set-default');
    expect(req.request.method).toBe('PUT');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });
});