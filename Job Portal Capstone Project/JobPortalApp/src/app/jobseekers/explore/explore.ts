import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JobSeekerService } from '../../services/user/job-seeker';

@Component({
  selector: 'app-explore',
  imports: [FormsModule, RouterLink, CommonModule, CurrencyPipe],
  templateUrl: './explore.html',
  styleUrl: './explore.css'
})
export class Explore implements OnInit {
  searchTerm: string = "";
  allJobs: any[] = [];
  filteredJobs: any[] = [];

  constructor(private JobSeekerService: JobSeekerService, private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem("JwtToken");

    const getJobDetails = () => {
      if (token) {
        this.JobSeekerService.getJobDetails().subscribe({
          next: (data) => {
            this.allJobs = data;
            this.filteredJobs = data;
          },
          error: (err) => {
            console.log(err);
          }
        });
      } else {
        setTimeout(getJobDetails, 100);
      }
    };

    getJobDetails();
  }

  onSearch() {
    const term = this.searchTerm.toLowerCase();
    this.filteredJobs = this.allJobs.filter(job =>
      job.title.toLowerCase().includes(term) ||
      job.companyName.toLowerCase().includes(term) ||
      job.requirements.toLowerCase().includes(term)
    );
  }

  borderColors = ['border-blue-500', 'border-green-500', 'border-yellow-400', 'border-purple-500'];

  getBorderClass(title: string): string {
    const index = title.length % this.borderColors.length;
    return this.borderColors[index];
  }
}