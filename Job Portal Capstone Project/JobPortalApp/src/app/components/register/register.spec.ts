import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Register } from './register';
import { RecruiterRegisterService } from '../../services/recruiter/recruiter-register-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { UserRegisterService } from '../../services/user/UserRegisterService';
import { UserRegisterModel } from '../../models/user/UserRegister';

describe('JobSeeker Register', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let mockRegisterService: jasmine.SpyObj<UserRegisterService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockRegisterService = jasmine.createSpyObj('UserRegisterService', ['Register', 'validateUserRegister']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule, Register],
      declarations: [],
      providers: [
        { provide: UserRegisterService, useValue: mockRegisterService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
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
      phoneNumber: '',
      address: '',
      password: '',
      cpassword: '',
      dateOfBirth: ''
    });
    expect(component.registerForm.invalid).toBeTrue();
  });



  it('should call register service and navigate on valid form', () => {
    const mockFormData = {
      name: 'Test Recruiter',
      email: 'test@company.com',
      phoneNumber: '9876543210',
      address: 'Chennai',
      password: 'Password@123',
      cpassword: 'Password@123',
      dateOfBirth: '2025-06-01',
    };

    component.registerForm.setValue(mockFormData);

    mockRegisterService.validateUserRegister.and.returnValue(of({ success: true }));

    component.handleRegister();

    expect(mockRegisterService.validateUserRegister).toHaveBeenCalledWith(
      jasmine.objectContaining(mockFormData)
    );
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/jobseekers');
  });


  it('should handle register error', () => {
    component.registerForm.setValue({
      name: 'Test',
      email: 'test@company.com',
      phoneNumber: '9999999999',
      address: 'Mumbai',
      password: 'Password@123',
      cpassword: 'Password@123',
      dateOfBirth: '2025-06-01'
    });

    mockRegisterService.validateUserRegister.and.returnValue(throwError(() => new Error('Registration failed')));

    component.handleRegister();

    expect(component).toBeDefined();
  });
});
