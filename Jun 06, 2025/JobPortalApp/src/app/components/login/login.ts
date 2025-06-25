import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserLoginModel } from '../../models/user/UserLogin';
import { CommonModule } from '@angular/common';
import { UserLoginService } from '../../services/user/UserLoginService';
import { Router } from '@angular/router';
import { textValidator } from '../../misc/TextValidator';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login implements OnInit {
  user: UserLoginModel = new UserLoginModel();
  loginForm: FormGroup;

  constructor(public userService: UserLoginService, public router: Router) {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, textValidator()]),
      cpassword: new FormControl('', Validators.required)
    })

  }

  ngOnInit(): void {
    const token = sessionStorage.getItem("JwtToken");
    if (token) {
      this.router?.navigateByUrl('/jobseekers')
    }
  }

  public get email(): any {
    return this.loginForm.get("email");
  }

  public get password(): any {
    return this.loginForm.get("password");
  }

  public get cpassword(): any {
    return this.loginForm.get("cpassword");
  }

  handleLogin() {
    this.user.email = this.email.value;
    this.user.password = this.password.value;
    this.user.cpassword = this.cpassword.value;
    if (this.loginForm.invalid) {
      return;
    }
    console.log(this.user);
    this.userService.validateUserLogin(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl('/jobseekers');
      },
      error: () => {
      }
    });
  }
}
