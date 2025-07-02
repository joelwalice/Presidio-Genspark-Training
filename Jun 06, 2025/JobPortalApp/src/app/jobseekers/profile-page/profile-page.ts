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
  appliedJobs: any[] = [];
  showToast: boolean = false;
  toastMessage: string = '';

  isEditing = false;
  message: string = "";

  getStatusText(status: number): string {
    switch (status) {
      case 0:
        return 'Applied';
      case 1:
        return 'Accepted';
      case 2:
        return 'Hired';
      case 3:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }

  editable = {
    name: '',
    email: '',
    phoneNumber: '',
    address: '',
    id : '',
    password: ''
  };


  constructor(private JobSeekerService: JobSeekerService, private router: Router) {

  }

  ngOnInit(): void {

    const email = localStorage.getItem('email');
    if (email) {
      this.JobSeekerService.fetchJobSeekerByEmail(email).subscribe({
        next: (user: any) => {
          this.fullName = user.name;
          this.email = user.email;
          this.phone = user.phoneNumber;
          this.location = user.address;
          this.editable = {
            name: user.name,
            email: user.email,
            phoneNumber: user.phoneNumber,
            address: user.address,
            id: user.id,
            password: user.password
          };
          localStorage.setItem("Id", user.id);
        },
        error: (err) => {
          console.error('Error fetching jobseeker:', err);
        }
      });
    }
    const appliedJobs = () => {
      const id = localStorage.getItem("Id");
      if (id) {

        this.JobSeekerService.getAppliedJobsByJobSeekerId(id).subscribe({
          next: (data) => {
            this.appliedJobs = data;
          },
          error: (err) => {
            console.error('Error fetching applied jobs:', err);
          }
        })
      }
      else {
        setTimeout(appliedJobs, 100);
      }
    };

    appliedJobs();
  }

  confirmDeleteAccount() {
    this.showDeleteConfirm = false;
    this.deleteAccount();
  }

  deleteAccount() {
    const Id = localStorage.getItem("Id");
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
    localStorage.clear();
    this.router.navigateByUrl('/login');
  }
  cancelEdit() {
    this.isEditing = false;
  }
  updateProfile() {
    this.fullName = this.editable.name;
    this.email = this.editable.email;
    this.phone = this.editable.phoneNumber;
    this.location = this.editable.address;
    this.isEditing = false;
    
    const id = localStorage.getItem("Id");
    if (!id) {
      console.error('Job Seeker ID not found in local storage.');
      return;
    }
    this.JobSeekerService.updateProfile(this.editable).subscribe({
      next: (data) => {
        this.showToast = true;
        this.toastMessage = 'Profile updated successfully!';
        setTimeout(() => {
          this.showToast = false;
          this.toastMessage = '';
        }, 3000);
      },
      error: (err) => {
        console.error('Error updating profile:', err);
      }
    });
  }
}
