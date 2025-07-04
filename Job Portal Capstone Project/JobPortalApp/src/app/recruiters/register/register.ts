import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { textValidator } from '../../misc/TextValidator';
import { RecruiterLoginService } from '../../services/recruiter/recruiter-login-service';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RecruiterRegisterService } from '../../services/recruiter/recruiter-register-service';

@Component({
  selector: 'app-recruiter-register',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RecruiterRegister {
  registerForm: FormGroup;

  constructor(public RecruiterRegisterService: RecruiterRegisterService, public router: Router) {
    this.registerForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, textValidator()]),
      cpassword: new FormControl(null, [Validators.required]),
      phoneNumber: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
      address: new FormControl(null, [Validators.required]),
      dateOfBirth: new FormControl(null, [Validators.required]),
      companyName: new FormControl(null, [Validators.required])
    });
  }

  public get name() {
    return this.registerForm.get('name');
  }

  public get email() {
    return this.registerForm.get('email');
  }

  public get password() {
    return this.registerForm.get('password');
  }

  public get cpassword() {
    return this.registerForm.get('cpassword');
  }

  public get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }

  public get address() {
    return this.registerForm.get('address');
  }

  public get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }

  public get companyName() {
    return this.registerForm.get('companyName');
  }

  handleRegister() {
    if (this.registerForm.invalid || this.password?.value !== this.cpassword?.value) {
      return;
    }

    const recruiterPayload = {
      name: this.name?.value,
      email: this.email?.value,
      password: this.password?.value,
      cpassword: this.cpassword?.value,
      phoneNumber: this.phoneNumber?.value,
      address: this.address?.value,
      dateOfBirth: this.dateOfBirth?.value,
      companyName: this.companyName?.value
    };

    this.RecruiterRegisterService.validateRecruiterRegister(recruiterPayload).subscribe({
      next : () => {
        console.log("Registration successful");
        this.router.navigateByUrl('/recruiters/home');
      },
      error : () => {
        
      }
    })
  }
}
