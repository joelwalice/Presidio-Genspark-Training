import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, HostListener, inject, OnInit, ViewChild } from '@angular/core';
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

  @ViewChild('dropdown') dropdownRef!: ElementRef;
  @ViewChild('toggleBtn') toggleBtnRef!: ElementRef;

  constructor(private JobSeekerService: JobSeekerService, private router: Router) {

  }

  logout() {
    localStorage.clear();
    this.router.navigateByUrl('/login');
  }

  ngOnInit(): void {
    const checkEmailAndFetch = () => {
      const email = localStorage.getItem('email');
      if (email) {
        this.email = email;
        this.JobSeekerService.fetchJobSeekerByEmail(email).subscribe({
          next: (user: any) => {
            localStorage.setItem("Id", user.id);
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

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent): void {
    const clickedInsideDropdown = this.dropdownRef?.nativeElement.contains(event.target);
    const clickedToggleBtn = this.toggleBtnRef?.nativeElement.contains(event.target);

    if (!clickedInsideDropdown && !clickedToggleBtn) {
      this.isDropdownOpen = false;
    }
  }

}
