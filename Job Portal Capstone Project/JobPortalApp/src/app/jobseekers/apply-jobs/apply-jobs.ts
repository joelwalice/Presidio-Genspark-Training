import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobSeekerService } from '../../services/user/job-seeker';
import { FormsModule } from '@angular/forms';
import { ResumeStateService } from '../../services/user/resume-state-service';

@Component({
  selector: 'app-apply-jobs',
  imports: [DatePipe, CommonModule, FormsModule],
  templateUrl: './apply-jobs.html',
  styleUrl: './apply-jobs.css'
})
export class ApplyJobs {
  jobId!: string;
  jobs = {
    id: '',
    title: "xUnit Tester",
    description: "Personnel to be knowledged in xUnit Testing, Application Testing",
    location: "USA",
    companyName: "Presidio Pvt Ltd",
    salary: 600000,
    requirements: "Unit Testing in ASP.Net",
    expiryDate: ''
  };
  resumes: any[] = [];
  selectedResumeId: string = '';
  message: string = '';
  jobSeekerId: string = localStorage.getItem("Id") || '';

  constructor(private JobSeekerService: JobSeekerService, private route: ActivatedRoute, private router: Router, private resumeState: ResumeStateService) { }

  ngOnInit(): void {
    this.jobId = this.route.snapshot.paramMap.get('id') || '';
    const getJobDetailById = () => {
      if (this.jobId) {
        this.JobSeekerService.getJobDetailById(this.jobId).subscribe({
          next: (data: any) => {
            this.jobs = data;
          }
        })
      }
      else {
        setTimeout(getJobDetailById, 100);
      }
    }
    getJobDetailById();
    const getResumesByJobseekerId = () => {
      if (this.jobSeekerId) {
        this.JobSeekerService.getResumesByJobseekerId(this.jobSeekerId).subscribe({
          next: (data) => {
            this.resumes = data;
          }
        })
      }
      else {
        setTimeout(getResumesByJobseekerId, 100);
      }
    }
    getResumesByJobseekerId();
  }
  applyToJob() {
    if (!this.selectedResumeId) {
      alert("Please select a resume.");
      return;
    }

    const payload = {
      jobId: this.jobId,
      jobSeekerId: this.jobSeekerId,
      resumeDocumentId: this.selectedResumeId
    };

    this.JobSeekerService.applyJobs(payload).subscribe({
      next: (data) => {
        this.message = data.message;
        this.resumeState.notifyAppliedJobChange();
        setTimeout(() => {
          this.router.navigateByUrl('/jobseekers');
        }, 2000);
      },
      error: (err) => {
        this.message = err.error?.error || "Something went wrong. Please try again.";
      }
    })

  }
}
