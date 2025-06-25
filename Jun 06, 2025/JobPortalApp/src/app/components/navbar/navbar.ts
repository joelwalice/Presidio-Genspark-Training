import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, HostListener, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { JobSeekerService } from '../../services/user/job-seeker';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {
  email: string = '';

  isDropdownOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  constructor(private JobSeekerService: JobSeekerService, private router: Router) {

  }

  logout() {
    sessionStorage.clear();
    this.router.navigateByUrl('/login');
  }

  ngOnInit(): void {
    const checkEmailAndFetch = () => {
      const email = sessionStorage.getItem('email');
      if (email) {
        this.email = email;
        this.JobSeekerService.fetchJobSeekerByEmail(email).subscribe({
          next: (user: any) => {
            console.log(user);
            sessionStorage.setItem("Id", user.id);
          },
          error: (err) => {
            console.error('Error fetching jobseeker:', err);
          }
        });
      } else {
        setTimeout(checkEmailAndFetch, 100);
      }
    };

    checkEmailAndFetch();
  }

}
