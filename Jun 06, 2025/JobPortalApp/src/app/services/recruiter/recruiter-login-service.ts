import { inject, Injectable } from '@angular/core';
import { RecruiterLoginModel } from '../../models/recruiter/RecruiterLogin';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RecruiterLoginService {

  constructor() { }
  private http = inject(HttpClient);
    public errorMessage: string = '';
    validateUserLogin(user: RecruiterLoginModel): Observable<any> {
        if (user.password.length < 6) {
            this.errorMessage = "Password must be at least 6 characters.";
            return throwError(() => new Error(this.errorMessage));
        }

        return this.callLoginAPI(user).pipe(
            tap((data: any) => {
                localStorage.setItem("JwtToken", data.token);
                localStorage.setItem("email", data.email);
            }),
            tap((data: any) => {
                if (data.role !== 'Recruiter') {
                    this.errorMessage = "You are not authorized to login as a Recruiter.";
                    throw new Error(this.errorMessage);
                }
                localStorage.setItem("role", "Recruiter");
                this.errorMessage = '';
            }),
            catchError((err) => {
                this.errorMessage = this.errorMessage = err.error.message || "Something came up! Please try again later.";
                return throwError(() => err);
            })
        );
    }

    private callLoginAPI(user: RecruiterLoginModel): Observable<any> {
        return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
            email: user.email,
            role: 'Recruiter',
            password: user.password
        });
    }
}
