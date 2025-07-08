import { inject, Injectable } from '@angular/core';
import { RecruiterRegisterModel } from '../../models/recruiter/RecruiterRegister';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecruiterRegisterService {

  constructor() { }
  private http = inject(HttpClient);
  public errorMessage: string = '';
  validateRecruiterRegister(user: RecruiterRegisterModel): Observable<any> {
    if (user.password.length < 6) {
      this.errorMessage = "Password must be at least 6 characters.";
      return throwError(() => new Error(this.errorMessage));
    }

    if (user.password !== user.cpassword) {
      this.errorMessage = "Passwords do not match.";
      return throwError(() => new Error(this.errorMessage));
    }
    return this.callRegisterAPI(user).pipe(
      switchMap(() => this.callLoginAPI(user)),
      tap((loginData: any) => {
        localStorage.setItem("JwtToken", loginData.token);
        localStorage.setItem("email", loginData.email);
        localStorage.setItem("role", "Recruiter");
        this.errorMessage = '';
      }),
      catchError((err) => {
        this.errorMessage = err?.error?.message || "Something went wrong. Please try again.";
        return throwError(() => new Error(this.errorMessage));
      })
    )
  }

  private callRegisterAPI(user: RecruiterRegisterModel): Observable<any> {
    return this.http.post(`http://localhost:5039/api/recruiter`, {
      name: user.name,
      email: user.email,
      password: user.password,
      address: user.address,
      dateOfBirth: new Date(user.dateOfBirth).toISOString(),
      phoneNumber: user.phoneNumber,
      companyName: user.companyName
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  private callLoginAPI(user: RecruiterRegisterModel): Observable<any> {
    return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
      email: user.email,
      role: 'Recruiter',
      password: user.password
    });
  }

  
}
