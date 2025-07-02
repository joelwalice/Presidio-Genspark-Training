import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, map } from 'rxjs';
import { JobSeekerService } from '../../services/user/job-seeker';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-recruiter-applicants',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './recruiter-applicants.html',
  styleUrl: './recruiter-applicants.css'
})
export class RecruiterApplicants implements OnInit {
  applicants: any[] = [];
  jobId: string = '';
  selectedApplicant: any = null;
  showModal: boolean = false;
  action: string = '';
  errorMessage: string = '';

  constructor(private JobSeekerService: JobSeekerService, private recruiterService: RecruiterService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.jobId = this.route.snapshot.paramMap.get('jobId') || '';
    if (this.jobId) {
      this.fetchApplicants(this.jobId);
    }
    else {
      setTimeout(this.fetchApplicants.bind(this), 1000);
    }
  }

  updateStatus(applicant: any) {
    console.log('Updating status for applicant:', applicant);
    this.recruiterService.updateApplicationStatus(applicant.jobSeekerId, applicant.jobStatus).subscribe({
      next: () => {
        console.log('Status updated');
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to update status for Hired Applicants';
        console.error('Error updating status:', err)
      }
    });
  }


  fetchApplicants(jobId: string) {
    this.recruiterService.getJobApplicationsByJobId(jobId).subscribe({
      next: (applications: any[]) => {
        const fetches = applications.map(app =>
          this.JobSeekerService.getJobSeekerById(app.jobSeekerId).pipe(
            map(jobseeker => ({
              ...app,
              jobSeeker: jobseeker
            }))
          )
        );

        forkJoin(fetches).subscribe({
          next: (mergedApplicants) => {
            const resumeFetches = mergedApplicants.map(applicant =>
              this.JobSeekerService.getResumeById(applicant.resumeDocumentId).pipe(
                map(resume => ({
                  ...applicant,
                  resumeContent: resume.FileContent,
                  resumeFileName: resume.FileName,
                  resumeFileType: resume.FileType
                }))
              )
            );

            forkJoin(resumeFetches).subscribe({
              next: (finalApplicants) => {
                console.log('Final Applicants:', finalApplicants);
                this.applicants = finalApplicants;
              },
              error: (err) => console.error('Error fetching resumes:', err)
            });
          },
          error: (err) => console.error('Error fetching applicants:', err)
        });
      },
      error: (err) => console.error('Error loading applications:', err)
    });
  }

  viewDetails(applicant: any) {
    this.selectedApplicant = applicant;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedApplicant = null;
  }

  downloadResume(applicant: any) {
    const byteCharacters = atob(applicant.resumeContent);
    const byteNumbers = Array.from(byteCharacters, char => char.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: applicant.resumeFileType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = applicant.resumeFileName;
    a.click();
    URL.revokeObjectURL(url);
  }
}
