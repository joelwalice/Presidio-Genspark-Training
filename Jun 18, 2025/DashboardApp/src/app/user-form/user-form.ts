import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/UserService';

@Component({
  selector: 'app-user-form',
  imports: [ReactiveFormsModule],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css'
})
export class UserForm {
  UserForm : FormGroup;
    constructor(private userService:UserService,private router:Router){
      this.UserForm = new FormGroup({
        firstName:new FormControl(null,Validators.required),
        lastName:new FormControl(null,Validators.required),
        role:new FormControl(null,Validators.required),
        gender: new FormControl(null, Validators.required)
      })
  }
  public get firstName() : any {
    return this.UserForm.get("firstName")
  }
  public get lastName() : any {
    return this.UserForm.get("lastNname")
  }
  public get role() : any {
    return this.UserForm.get("role")
  }
  public get gender() : any{
    return this.UserForm.get("gender")
  }
  handleSubmit(){
    console.log(this.UserForm.value);
    if(this.UserForm.valid){
      this.userService.addUser(this.UserForm.value).subscribe({
        next : (data) => {
          console.log("User Data Added : ", data);
          alert("User Added Successfully!!");
          this.UserForm.reset();
          this.router.navigateByUrl('user')
        },
        error : (err) => {
          console.log("Error ", err);
        }
      })
    }
  }

}
