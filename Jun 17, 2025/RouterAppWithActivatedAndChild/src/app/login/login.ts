import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserLoginModel } from '../models/UserLoginModel';
import { UserService } from '../services/UserService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user:UserLoginModel = new UserLoginModel();
  constructor(private userService:UserService,private route:Router){

  }
  handleLogin(){
    this.userService.validateUserLogin(this.user);
    this.route.navigateByUrl("/user/"+this.user.username);
  }
}
