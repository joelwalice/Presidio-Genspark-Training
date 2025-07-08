import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RecruiterService } from '../../services/recruiter/recruiter-service';

@Component({
  selector: 'app-recruiter-profile-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './recruiter-profile-page.html',
  styleUrl: './recruiter-profile-page.css'
})
export class RecruiterProfilePage implements OnInit {
  fullName = 'Guest Name';
  email = 'guest@example.com';
  phone = '+91 98765 43210';
  location = 'Chennai, India';
  companyName = 'Not Available';
  showDeleteConfirm: boolean = false;
  isEditing = false;
  message: string = "";

  editable = {
    name: '',
    email: '',
    phoneNumber: '',
    address: '',
    id: '',
    password: ''
  };

  constructor(private recruiterService: RecruiterService, private router: Router) {}

  ngOnInit(): void {
    const email = localStorage.getItem('email');
    if (email) {
      this.recruiterService.fetchRecruiterByEmail(email).subscribe({
        next: (user: any) => {
          this.fullName = user.name;
          this.email = user.email;
          this.phone = user.phoneNumber;
          this.location = user.address;
          this.recruiterService.getCompanyById(user.companyId).subscribe({
            next: (data) => {
              this.companyName = data.name;
            }
          })
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
        error: (err) => console.error('Error fetching recruiter:', err)
      });
    }
  }

  confirmDeleteAccount() {
    this.showDeleteConfirm = false;
    this.deleteAccount();
  }

  deleteAccount() {
    const Id = localStorage.getItem("Id");
    if (Id) {
      this.recruiterService.deleteRecruiter(Id).subscribe({
        next: () => this.logout()
      });
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
    this.router.navigateByUrl('/recruiters');
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
      console.error('Recruiter ID not found in local storage.');
      return;
    }
    this.recruiterService.updateProfile(this.editable).subscribe({
      next: (data) => {
        this.message = 'Profile updated successfully!';
        setTimeout(() => {
          this.message = '';
        }, 3000);
        console.log('Profile updated:', data);
      },
      error: (err) => console.error('Error updating profile:', err)
    });
  }
}
