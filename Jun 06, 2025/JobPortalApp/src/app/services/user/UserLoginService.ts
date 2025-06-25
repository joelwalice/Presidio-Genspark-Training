import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { UserLoginModel } from "../../models/user/UserLogin";

@Injectable()
export class UserLoginService {
    private http = inject(HttpClient);

    validateUserLogin(user: UserLoginModel) {
        console.log(user.password, user.cpassword)
        if (user.password.length < 6) {
            console.log("Password is not taken");
            return;
        }
        if (user.password != user.cpassword) {
            alert("Password entries doesn't match with the requirements!!")
        }

        else {
            this.callLoginAPI(user).subscribe(
                {
                    next: (data: any) => {
                        sessionStorage.setItem("JwtToken", data.token);
                        sessionStorage.setItem("email", data.email);
                    }
                }
            )

        }

    }

    callLoginAPI(user: UserLoginModel) {
        return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
            email: user.email,
            role: 'JobSeeker',
            password: user.password
        })
    }
}