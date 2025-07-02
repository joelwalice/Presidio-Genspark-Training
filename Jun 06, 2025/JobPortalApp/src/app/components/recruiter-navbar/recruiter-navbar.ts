import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { RecruiterService } from '../../services/recruiter/recruiter-service';

@Component({
  selector: 'app-recruiter-navbar',
  imports: [CommonModule, RouterLink],
  templateUrl: './recruiter-navbar.html',
  styleUrl: './recruiter-navbar.css'
})
export class RecruiterNavbar {
  email: string = '';

  isDropdownOpen = false;

  @ViewChild('dropdown') dropdownRef!: ElementRef;
  @ViewChild('toggleBtn') toggleBtnRef!: ElementRef;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  constructor(private RecruiterService : RecruiterService, private router: Router) {

  }

  logout() {
    localStorage.clear();
    this.router.navigateByUrl('/recruiters');
  }

  ngOnInit(): void {
    const checkEmailAndFetch = () => {
      const email = localStorage.getItem('email');
      if (email) {
        this.email = email;
        this.RecruiterService.fetchJobSeekerByEmail(email).subscribe({
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
