import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../services/UserService';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  username$:any;
  usrname:string|null = "";

  constructor(private userService:UserService)
  {
    this.userService.username$.subscribe(
      {
       next:(value) =>{
          this.usrname = value ;
        },
        error:(err)=>{
          alert(err);
        }
      }
    )
  }
}
