import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css'
})
export class HomePage implements OnInit {
  ngOnInit(): void {
    const token = localStorage.getItem("JwtToken");
    const role = localStorage.getItem("role");
    if(token && role == "JobSeeker"){
      this.router?.navigateByUrl('/jobseekers')
    }
    if(token && role == "Recruiter"){
      this.router?.navigateByUrl('/recruiters/home')
    }
  }
  constructor(private router : Router){
    
  }
}
