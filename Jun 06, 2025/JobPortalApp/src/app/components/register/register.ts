import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRegisterModel } from '../../models/user/UserRegister';
import { UserRegisterService } from '../../services/user/UserRegisterService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register implements OnInit {
  user : UserRegisterModel = new UserRegisterModel();
  registerForm : FormGroup;
  
  ngOnInit(): void {
    const token = localStorage.getItem("JwtToken");
    if(token){
      this.router?.navigateByUrl('/jobseekers')
    }
  }
  
  constructor(public UserService : UserRegisterService, private router : Router){
    this.registerForm = new FormGroup({
      name : new FormControl(null, Validators.required),
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required]),
      cpassword: new FormControl(null, Validators.required),
      address : new FormControl(null, Validators.required),
      dateOfBirth : new FormControl(null, Validators.required),
      phoneNumber : new FormControl(null, Validators.required),
    })
  }

  public get name(): any {
    return this.registerForm.get("name");
  }

  public get email(): any {
    return this.registerForm.get("email");
  }

  public get password(): any {
    return this.registerForm.get("password");
  }

  public get cpassword(): any {
    return this.registerForm.get("cpassword");
  }

  public get address(): any {
    return this.registerForm.get("address");
  }

  public get dateOfBirth(): any {
    return this.registerForm.get("dateOfBirth");
  }

  public get phoneNumber(): any {
    return this.registerForm.get("phoneNumber");
  }

  handleRegister(){
    this.user.name = this.name.value;
    this.user.email = this.email.value;
    this.user.password = this.password.value;
    this.user.address = this.address.value;
    this.user.dateOfBirth = this.dateOfBirth.value;
    this.user.cpassword = this.cpassword.value;
    this.user.phoneNumber = this.phoneNumber.value;
    this.UserService.validateUserRegister(this.user).subscribe({
      next : () => {
        this.router.navigateByUrl('/jobseekers');
      },
      error : () => {
        
      }
    })
  }
}
