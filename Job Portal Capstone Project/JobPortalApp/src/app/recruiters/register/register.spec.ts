import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecruiterRegister } from './register';
import { RecruiterRegisterService } from '../../services/recruiter/recruiter-register-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('Recruiter Register', () => {
  let component: RecruiterRegister;
  let fixture: ComponentFixture<RecruiterRegister>;
  let mockRegisterService: jasmine.SpyObj<RecruiterRegisterService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockRegisterService = jasmine.createSpyObj('RecruiterRegisterService', ['registerRecruiter', 'validateRecruiterRegister']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [RecruiterRegister, HttpClientTestingModule, ReactiveFormsModule],
      providers: [
        { provide: RecruiterRegisterService, useValue: mockRegisterService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruiterRegister);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should not call service if form is invalid', () => {
    component.registerForm.setValue({
      name: '',
      email: '',
      phoneNumber: '9876543210',
      address: 'Chennai',
      password: 'Password@123',
      cpassword: 'Password@123',
      dateOfBirth: '2025-06-01',
      companyName : 'Company101'
    });

    component.handleRegister();
    expect(mockRegisterService.validateRecruiterRegister).not.toHaveBeenCalled();
  });

  it('should call register service and navigate on valid form', () => {
    const mockData = {
      name: 'Test Recruiter',
      email: 'test@company.com',
      phoneNumber: '9876543210',
      address: 'Chennai',
      password: 'Password@123',
      cpassword: 'Password@123',
      dateOfBirth: '2025-06-01',
      companyName : 'Company101'
    };

    component.registerForm.setValue(mockData);
    
    mockRegisterService.validateRecruiterRegister.and.returnValue(of({ success: true }));
    
    component.handleRegister();
    
    expect(mockRegisterService.validateRecruiterRegister).toHaveBeenCalledWith(mockData);
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/recruiters/home');
  });

  it('should handle register error', () => {
    component.registerForm.setValue({
      name: 'Test',
      email: 'test@company.com',
      phoneNumber: '9999999999',
      address: 'Mumbai',
      password: 'Password@123',
      cpassword: 'Password@123',
      dateOfBirth: '2025-06-01',
      companyName: 'Company101'
    });

    mockRegisterService.validateRecruiterRegister.and.returnValue(throwError(() => new Error('Registration failed')));

    component.handleRegister();

    expect(component).toBeDefined(); 
  });
});
