import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { textValidator } from '../../misc/TextValidator';
import { RecruiterLoginService } from '../../services/recruiter/recruiter-login-service';
import { Router, RouterOutlet } from '@angular/router';
import { RecruiterLoginModel } from '../../models/recruiter/RecruiterLogin';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterOutlet],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class RecruiterLogin {
  user: RecruiterLoginModel = new RecruiterLoginModel();
  loginForm: FormGroup;

  get isOnRecruiterLoginPage(): boolean {
    return this.router.url === '/recruiters';
  }

  constructor(public userService: RecruiterLoginService, public router: Router) {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, textValidator()]),
    })

  }

  ngOnInit(): void {
    const token = localStorage.getItem("JwtToken");
    const role = localStorage.getItem("role");
    if (token && role == "JobSeeker") {
      this.router?.navigateByUrl('/jobseekers')
    }
    if (token && role == "Recruiter") {
      this.router?.navigateByUrl('/recruiters/home')
    }
    
  }

  public get email(): any {
    return this.loginForm.get("email");
  }

  public get password(): any {
    return this.loginForm.get("password");
  }

  handleLogin() {
    this.user.email = this.email.value;
    this.user.password = this.password.value;
    if (this.loginForm.invalid) {
      return;
    }
    console.log(this.user);
    this.userService.validateUserLogin(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl('/recruiters/home');
      },
      error: () => {
      }
    });
  }
}
