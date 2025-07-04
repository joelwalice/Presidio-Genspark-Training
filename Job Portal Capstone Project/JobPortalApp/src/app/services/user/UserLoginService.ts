import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, tap, throwError } from "rxjs";
import { UserLoginModel } from "../../models/user/UserLogin";

@Injectable()
export class UserLoginService {
    private http = inject(HttpClient);
    public errorMessage: string = '';
    validateUserLogin(user: UserLoginModel): Observable<any> {
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
                if (data.role !== 'JobSeeker') {
                    this.errorMessage = "You are not authorized to login as a Job Seeker.";
                    throw new Error(this.errorMessage);
                }
                localStorage.setItem("role", "JobSeeker");
                this.errorMessage = '';
            }),
            catchError((err) => {
                this.errorMessage = err.error.message || "Something went wrong. Please try again later.";
                return throwError(() => err);
            })
        );
    }

    private callLoginAPI(user: UserLoginModel): Observable<any> {
        return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
            email: user.email,
            role: 'JobSeeker',
            password: user.password
        });
    }
}