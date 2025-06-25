import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JobSeekerService } from '../../services/user/job-seeker';
import { ResumePage } from "../resume-page/resume-page";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile-page',
  imports: [ResumePage, CommonModule, FormsModule],
  templateUrl: './profile-page.html',
  styleUrl: './profile-page.css'
})
export class ProfilePage implements OnInit {
  fullName = 'Guest Name';
  email = 'guest@example.com';
  phone = '+91 98765 43210';
  location = 'Chennai, India';
  role = 'Frontend Developer';
  showDeleteConfirm: boolean = false;

  isEditing = false;

  editable = {
    fullName: '',
    email: '',
    phone: '',
    location: ''
  };
  

  constructor(private JobSeekerService: JobSeekerService, private router: Router) {

  }

  ngOnInit(): void {

    const email = sessionStorage.getItem('email');
    if (email) {
      this.JobSeekerService.fetchJobSeekerByEmail(email).subscribe({
        next: (user: any) => {
          this.fullName = user.name;
          this.email = user.email;
          this.phone = user.phoneNumber;
          this.location = user.address;
          this.editable = {
            fullName: user.name,
            email: user.email,
            phone: user.phoneNumber,
            location: user.address
          };
          console.log(user);
          sessionStorage.setItem("Id", user.id);
        },
        error: (err) => {
          console.error('Error fetching jobseeker:', err);
        }
      });
    }

  }

  confirmDeleteAccount() {
    this.showDeleteConfirm = false;
    this.deleteAccount();
  }

  deleteAccount() {
    const Id = sessionStorage.getItem("Id");
    if (Id) {
      this.JobSeekerService.deleteJobSeeker(Id).subscribe({
        next: () => {
          this.logout();
        }
      })
    }
  }

  get userInitials(): string {
    return this.fullName
      .split(' ')
      .map((n) => n[0])
      .join('');
  }

  logout() {
    sessionStorage.clear();
    this.router.navigateByUrl('/login');
  }
  cancelEdit() {
    this.isEditing = false;
  }
  updateProfile() {
    this.fullName = this.editable.fullName;
    this.email = this.editable.email;
    this.phone = this.editable.phone;
    this.location = this.editable.location;
    this.isEditing = false;
  }
}
