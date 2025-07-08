import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { UserLoginService } from '../../services/user/UserLoginService';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { UserLoginModel } from '../../models/user/UserLogin';

describe('JobSeeker Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let mockLoginService: jasmine.SpyObj<UserLoginService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockLoginService = jasmine.createSpyObj('UserLoginService', ['validateUserLogin']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [Login, HttpClientTestingModule],
      providers: [
        { provide: UserLoginService, useValue: mockLoginService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have email and password form controls', () => {
    expect(component.email).toBeTruthy();
    expect(component.password).toBeTruthy();
  });

  it('should mark form as invalid when inputs are empty', () => {
    component.loginForm.setValue({ email: '', password: '' });
    expect(component.loginForm.invalid).toBeTrue();
  });

  it('should call login service and navigate on valid login', () => {
    component.loginForm.setValue({ email: 'test@email.com', password: 'Valid123' });
    mockLoginService.validateUserLogin.and.returnValue(of({ token: 'fake-token' }));

    component.handleLogin();

    expect(mockLoginService.validateUserLogin).toHaveBeenCalledWith(
      jasmine.any(UserLoginModel)
    );
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/jobseekers');
  });

  it('should not call login service if form is invalid', () => {
    component.loginForm.setValue({ email: '', password: '' });

    component.handleLogin();

    expect(mockLoginService.validateUserLogin).not.toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
  });

  it('should handle login error gracefully', () => {
    component.loginForm.setValue({ email: 'test@email.com', password: 'Valid123' });
    mockLoginService.validateUserLogin.and.returnValue(throwError(() => new Error('Login failed')));

    component.handleLogin();

    expect(mockLoginService.validateUserLogin).toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
  });
});
