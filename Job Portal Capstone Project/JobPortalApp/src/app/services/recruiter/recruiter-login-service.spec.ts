import { TestBed } from '@angular/core/testing';
import { RecruiterLoginService } from './recruiter-login-service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RecruiterLoginModel } from '../../models/recruiter/RecruiterLogin';

describe('Recruiter Login Service', () => {
  let service: RecruiterLoginService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule],
      providers: [RecruiterLoginService]
    });
    service = TestBed.inject(RecruiterLoginService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return error if password is less than 6 characters', (done) => {
    const user: RecruiterLoginModel = { email: 'test@email.com', password: '123' };
    service.validateUserLogin(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(err.message).toBe('Password must be at least 6 characters.');
        expect(service.errorMessage).toBe('Password must be at least 6 characters.');
        done();
      }
    });
  });

  it('should call API and store token/email/role on valid login', (done) => {
    const user: RecruiterLoginModel = { email: 'test@email.com', password: '123456' };
    const mockResponse = { token: 'abc123', email: 'test@email.com', role: 'Recruiter' };

    service.validateUserLogin(user).subscribe((data) => {
      expect(localStorage.getItem('JwtToken')).toBe('abc123');
      expect(localStorage.getItem('email')).toBe('test@email.com');
      expect(localStorage.getItem('role')).toBe('Recruiter');
      expect(service.errorMessage).toBe('');
      done();
    });

    const req = httpMock.expectOne('http://localhost:5039/api/Authentication/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should set error if role is not Recruiter', (done) => {
    const user: RecruiterLoginModel = { email: 'test@email.com', password: '123456' };
    const mockResponse = { token: 'abc123', email: 'test@email.com', role: 'OtherRole' };

    service.validateUserLogin(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(service.errorMessage).toBe('You are not authorized to login as a Recruiter.');
        done();
      }
    });

    const req = httpMock.expectOne('http://localhost:5039/api/Authentication/auth/login');
    req.flush(mockResponse);
  });

  it('should set errorMessage on API error', (done) => {
    const user: RecruiterLoginModel = { email: 'test@email.com', password: '123456' };
    const mockError = { status: 401, statusText: 'Unauthorized', error: { message: 'Invalid credentials' } };

    service.validateUserLogin(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(service.errorMessage).toBe('Invalid credentials');
        done();
      }
    });

    const req = httpMock.expectOne('http://localhost:5039/api/Authentication/auth/login');
    req.flush(mockError.error, mockError);
  });
});