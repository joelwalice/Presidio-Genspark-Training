// src/app/interceptors/auth.interceptor.ts
import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const AuthInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const router = inject(Router);
  const token = localStorage.getItem('JwtToken');

  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const role = localStorage.getItem('role');
        localStorage.removeItem('JwtToken');
        localStorage.removeItem('role');
        localStorage.removeItem('Id');
        localStorage.removeItem('email');
        console.log('Session expired. Please log in again.');
        if(role === 'Recruiter') {
          router.navigate(['/recruiters']);
        } else if(role === 'JobSeeker') {
          router.navigate(['/jobseekers']);
        } else {
          // Default to login page if role is not recognized
        router.navigate(['/login']);
        }
      }
      return throwError(() => error);
    })
  );
};
