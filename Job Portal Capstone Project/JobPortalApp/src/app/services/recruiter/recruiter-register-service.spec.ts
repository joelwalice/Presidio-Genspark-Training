import { TestBed } from '@angular/core/testing';
import { RecruiterRegisterService } from './recruiter-register-service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RecruiterRegisterModel } from '../../models/recruiter/RecruiterRegister';

describe('Recruiter Register Service', () => {
  let service: RecruiterRegisterService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule],
      providers: [RecruiterRegisterService]
    });
    service = TestBed.inject(RecruiterRegisterService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return error if password is less than 6 characters', (done) => {
    const user: RecruiterRegisterModel = {
      name: 'Test',
      email: 'test@email.com',
      password: '123',
      cpassword: '123',
      address: 'Address',
      dateOfBirth: '2000-01-01',
      phoneNumber: '1234567890',
      companyName: 'TestCompany'
    };
    service.validateRecruiterRegister(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(err.message).toBe('Password must be at least 6 characters.');
        expect(service.errorMessage).toBe('Password must be at least 6 characters.');
        done();
      }
    });
  });

  it('should return error if passwords do not match', (done) => {
    const user: RecruiterRegisterModel = {
      name: 'Test',
      email: 'test@email.com',
      password: '123456',
      cpassword: '654321',
      address: 'Address',
      dateOfBirth: '2000-01-01',
      phoneNumber: '1234567890',
      companyName: 'TestCompany'
    };
    service.validateRecruiterRegister(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(err.message).toBe('Passwords do not match.');
        expect(service.errorMessage).toBe('Passwords do not match.');
        done();
      }
    });
  });

  it('should call register and login API and store data on valid registration', (done) => {
    const user: RecruiterRegisterModel = {
      name: 'Test',
      email: 'test@email.com',
      password: '123456',
      cpassword: '123456',
      address: 'Address',
      dateOfBirth: '2000-01-01',
      phoneNumber: '1234567890',
      companyName: 'TestCompany'
    };
    const mockLoginResponse = { token: 'abc123', email: 'test@email.com', role: 'Recruiter' };

    service.validateRecruiterRegister(user).subscribe((data) => {
      expect(localStorage.getItem('JwtToken')).toBe('abc123');
      expect(localStorage.getItem('email')).toBe('test@email.com');
      expect(localStorage.getItem('role')).toBe('Recruiter');
      expect(service.errorMessage).toBe('');
      done();
    });

    const reqRegister = httpMock.expectOne('http://localhost:5039/api/recruiter');
    expect(reqRegister.request.method).toBe('POST');
    reqRegister.flush({});

    const reqLogin = httpMock.expectOne('http://localhost:5039/api/Authentication/auth/login');
    expect(reqLogin.request.method).toBe('POST');
    reqLogin.flush(mockLoginResponse);
  });

  it('should set errorMessage on API error', (done) => {
    const user: RecruiterRegisterModel = {
      name: 'Test',
      email: 'test@email.com',
      password: '123456',
      cpassword: '123456',
      address: 'Address',
      dateOfBirth: '2000-01-01',
      phoneNumber: '1234567890',
      companyName: 'TestCompany'
    };
    const mockError = { status: 400, statusText: 'Bad Request', error: { message: 'Email already exists' } };

    service.validateRecruiterRegister(user).subscribe({
      next: () => {},
      error: (err) => {
        expect(service.errorMessage).toBe('Email already exists');
        done();
      }
    });

    const reqRegister = httpMock.expectOne('http://localhost:5039/api/recruiter');
    reqRegister.flush(mockError.error, mockError);
  });
});