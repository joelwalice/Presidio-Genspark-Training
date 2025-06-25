import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap, tap } from "rxjs/operators";
import { UserRegisterModel } from "../../models/user/UserRegister";

@Injectable()
export class UserRegisterService {
  private http = inject(HttpClient);
  public errorMessage: string | undefined;

  validateUserRegister(user: UserRegisterModel): Observable<any> {
    // Local validation
    if (user.password.length < 6) {
      this.errorMessage = "Password must be at least 6 characters.";
      return throwError(() => new Error(this.errorMessage));
    }

    if (user.password !== user.cpassword) {
      this.errorMessage = "Passwords do not match.";
      return throwError(() => new Error(this.errorMessage));
    }

    // If passed, call register API, then login
    return this.callRegisterAPI(user).pipe(
      tap((data: any) => {
        console.log("Registration successful:", data);
      }),
      switchMap(() => this.callLoginAPI(user)),
      tap((data: any) => {
        sessionStorage.setItem("JwtToken", data.token);
        sessionStorage.setItem("email", data.email);
        this.errorMessage = '';
      }),
      catchError((err) => {
        this.errorMessage = err?.error?.message || "Something went wrong. Please try again.";
        return throwError(() => new Error(this.errorMessage));
      })
    );
  }

  private callRegisterAPI(user: UserRegisterModel): Observable<any> {
    return this.http.post(`http://localhost:5039/api/jobseeker`, {
      name: user.name,
      email: user.email,
      password: user.password,
      address: user.address,
      dateOfBirth: new Date(user.dateOfBirth).toISOString(),
      phoneNumber: user.phoneNumber
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  private callLoginAPI(user: UserRegisterModel): Observable<any> {
    return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
      email: user.email,
      role: 'JobSeeker',
      password: user.password
    });
  }
}
