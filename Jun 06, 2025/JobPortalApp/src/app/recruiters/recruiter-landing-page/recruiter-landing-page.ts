import { Component, OnInit } from '@angular/core';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RecruiterNavbar } from "../../components/recruiter-navbar/recruiter-navbar";

@Component({
  selector: 'app-recruiter-landing-page',
  standalone: true,
  imports: [CommonModule, RecruiterNavbar, RouterLink, RouterOutlet],
  templateUrl: './recruiter-landing-page.html',
  styleUrl: './recruiter-landing-page.css'
})
export class RecruiterLandingPage implements OnInit {
  recruiterName: string = '';
  jobs: any[] = [];
  totalApplicants: number = 0;
  scheduledInterviews: number = 0;

  constructor(private recruiterService: RecruiterService, private router: Router) { }

  ngOnInit(): void {
    const token = localStorage.getItem('JwtToken');
    const role = localStorage.getItem('role');

    if (!token || role !== 'Recruiter') {
      window.location.href = '/recruiters';
      return;
    }

    this.waitForRecruiterIdAndFetch();
  }

  get isOnRecruiterDashboard(): boolean {
    return this.router.url === '/recruiters/home';
  }

  waitForRecruiterIdAndFetch(attempts: number = 0) {
    const recruiterId = localStorage.getItem('Id');

    if (recruiterId) {
      this.fetchRecruiterDetails(recruiterId);
      this.fetchPostedJobs();
    } else if (attempts < 20) {
      setTimeout(() => this.waitForRecruiterIdAndFetch(attempts + 1), 100);
    }
  }

  fetchRecruiterDetails(recruiterId: string) {
    this.recruiterService.getRecruiterById(recruiterId).subscribe({
      next: (data) => {
        this.recruiterName = data.name;
      },
      error: () => { }
    });
  }

  fetchPostedJobs() {
    const recruiterId = localStorage.getItem('Id');
    if (!recruiterId) return;

    this.recruiterService.getAllJobs().subscribe({
      next: (data: any[]) => {
        const jobsByRecruiter = data.filter(job => job.recruiterId === recruiterId);
        this.jobs = [];

        let totalApplicants = 0;
        let totalScheduled = 0;

        jobsByRecruiter.forEach(job => {
          this.recruiterService.getJobApplicationsByJobId(job.id).subscribe({
            next: (applications: any[]) => {
              const applicantCount = applications.length;
              const scheduledCount = applications.filter(a => a.status === 1).length;

              totalApplicants += applicantCount;
              totalScheduled += scheduledCount;

              this.jobs.push({
                ...job,
                applicantCount
              });

              this.totalApplicants = totalApplicants;
              this.scheduledInterviews = totalScheduled;
            },
            error: () => {
              this.jobs.push({ ...job, applicantCount: 0 });
            }
          });
        });
      },
      error: (err) => {
        console.error('Failed to fetch jobs:', err);
      }
    });
  }

}
