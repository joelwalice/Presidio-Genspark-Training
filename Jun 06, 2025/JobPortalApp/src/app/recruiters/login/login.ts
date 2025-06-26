import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { textValidator } from '../../misc/TextValidator';
import { RecruiterLoginService } from '../../services/recruiter/recruiter-login-service';
import { Router } from '@angular/router';
import { RecruiterLoginModel } from '../../models/recruiter/RecruiterLogin';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class RecruiterLogin {
  user: RecruiterLoginModel = new RecruiterLoginModel();
  loginForm: FormGroup;

  constructor(public userService: RecruiterLoginService, public router: Router) {
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
