import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { UserRegisterModel } from "../../models/user/UserRegister";

@Injectable()
export class UserRegisterService {
    private http = inject(HttpClient);

    validateUserRegister(user: UserRegisterModel) {
        console.log(user.password, user.cpassword)
        if (user.password.length < 6) {
            console.log("Password is not taken");
            return;
        }
        if (user.password != user.cpassword) {
            alert("Password entries doesn't match with the requirements!!")
        }

        else {
            this.callRegisterAPI(user).subscribe(
                {
                    next: (data: any) => {
                        console.log("Data : ", data);
                        this.callLoginAPI(user).subscribe(
                            {
                                next : (loginData : any) => {
                                    console.log(loginData);
                                    sessionStorage.setItem("JwtToken", data.token);
                                    sessionStorage.setItem("email", data.email);
                                    sessionStorage.setItem("Id", data.id);
                                }
                            }
                        )
                        
                    }
                }
            )

        }

    }

    callRegisterAPI(user: UserRegisterModel) {
        return this.http.post(`http://localhost:5039/api/jobseeker`, {
            name: user.name,
            email: user.email,
            password: user.password,
            address: user.address,
            dateOfBirth: new Date(user.dateOfBirth).toISOString(),
            phoneNumber: user.phoneNumber
        },{
            headers : {
                'Content-Type' : 'application/json'
            }
        });
    }

    callLoginAPI(user : UserRegisterModel){
        return this.http.post(`http://localhost:5039/api/Authentication/auth/login`, {
            email: user.email,
            role: 'JobSeeker',
            password: user.password
        })
    }
}