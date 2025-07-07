import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';

import { JobSeekerAuthGuard } from './jobseeker-auth-guard';

describe('JobSeekerAuthGuard', () => {
  let guard: JobSeekerAuthGuard;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      providers: [
        JobSeekerAuthGuard,
        { provide: Router, useValue: routerSpy }
      ]
    });
    guard = TestBed.inject(JobSeekerAuthGuard);
    localStorage.clear();
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should return false and navigate to login if not authenticated', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'JwtToken') return null;
      if (key === 'role') return null;
      return null;
    });
    const result = guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot);
    expect(result).toBeFalse();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['login']);
  });

  it('should return false and navigate to login if role is not JobSeeker', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'JwtToken') return 'token';
      if (key === 'role') return 'Employer';
      return null;
    });
    const result = guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot);
    expect(result).toBeFalse();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['login']);
  });

  it('should return true if authenticated and role is JobSeeker', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'JwtToken') return 'token';
      if (key === 'role') return 'JobSeeker';
      return null;
    });
    const result = guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot);
    expect(result).toBeTrue();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });
});
