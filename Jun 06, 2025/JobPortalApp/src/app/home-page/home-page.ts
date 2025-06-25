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
    const token = sessionStorage.getItem("JwtToken");
    if(token){
      this.router?.navigateByUrl('/jobseekers')
    }
  }
  constructor(private router : Router){
    
  }
}
