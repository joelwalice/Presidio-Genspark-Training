import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecruiterLogin } from './login';
import { RecruiterLoginService } from '../../services/recruiter/recruiter-login-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RecruiterLoginModel } from '../../models/recruiter/RecruiterLogin';

describe('Recruiter Login', () => {
  let component: RecruiterLogin;
  let fixture: ComponentFixture<RecruiterLogin>;
  let mockLoginService: jasmine.SpyObj<RecruiterLoginService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockLoginService = jasmine.createSpyObj('RecruiterLoginService', ['validateUserLogin']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [RecruiterLogin, HttpClientTestingModule],
      providers: [
        { provide: RecruiterLoginService, useValue: mockLoginService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruiterLogin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have form controls for email and password', () => {
    expect(component.email).toBeTruthy();
    expect(component.password).toBeTruthy();
  });

  it('should mark form invalid if fields are empty', () => {
    component.loginForm.setValue({ email: '', password: '' });
    expect(component.loginForm.invalid).toBeTrue();
  });

  it('should call login service and navigate if form is valid', () => {
    component.loginForm.setValue({ email: 'recruiter@test.com', password: 'Valid123' });
    mockLoginService.validateUserLogin.and.returnValue(of(true));
    component.handleLogin();

    expect(mockLoginService.validateUserLogin).toHaveBeenCalledWith(
      jasmine.any(RecruiterLoginModel)
    );
    const calledArg = mockLoginService.validateUserLogin.calls.mostRecent().args[0];
    expect(calledArg.email).toBe('recruiter@test.com');
    expect(calledArg.password).toBe('Valid123');
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/recruiters/home');
  });

  it('should not login or navigate if form is invalid', () => {
    component.loginForm.setValue({ email: '', password: '' });
    component.handleLogin();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
    component.loginForm.setValue({ email: 'recruiter@test.com', password: 'Invalid' });
    mockLoginService.validateUserLogin.and.returnValue(throwError(() => new Error('Invalid credentials')));
    component.handleLogin();

    expect(mockLoginService.validateUserLogin).toHaveBeenCalledWith(
      jasmine.any(RecruiterLoginModel)
    );

    expect(mockLoginService.validateUserLogin).toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
  });
});
