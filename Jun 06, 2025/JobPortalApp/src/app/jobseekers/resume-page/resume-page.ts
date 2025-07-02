import { Component, OnInit } from '@angular/core';
import { JobSeekerService } from '../../services/user/job-seeker';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { ResumeStateService } from '../../services/user/resume-state-service';

@Component({
  selector: 'app-resume-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './resume-page.html',
  styleUrl: './resume-page.css'
})
export class ResumePage implements OnInit {
  resumes: any[] = [];
  selectedResumeId: string = '';
  selectedResume: any = null;
  message: string = "";
  constructor(private JobSeekerService: JobSeekerService, private http: HttpClient, private sanitizer: DomSanitizer, private resumeState: ResumeStateService) { }

  ngOnInit(): void {
    const getResumeFn = () => {
      const id = localStorage.getItem("Id");
      if (id) {
        this.JobSeekerService.getResumesByJobseekerId(id).subscribe({
          next: (res) => {
            this.resumes = res;
            if (this.resumes.length > 0) {
              this.selectedResumeId = this.resumes[0].id;
              this.selectedResume = this.resumes[0];
              this.resumeState.setHasResume(true);
            } else {
              this.resumeState.setHasResume(false);
            }
          },
          error: (err) => console.error('Error fetching resumes:', err)
        });
      } else {
        setTimeout(getResumeFn, 100);
      }
    };
    getResumeFn();
  }

  onResumeChange(id: string) {
    this.selectedResume = this.resumes.find(r => r.id === id);
    const jobSeekerId = localStorage.getItem('Id');
    if (!jobSeekerId) return;

    this.JobSeekerService.setDefaultResume(jobSeekerId, id).subscribe({
      next: () => this.message = 'Default resume set successfully.',
      error: err => this.message = 'Error setting default resume: ' + err.message
    });
  }

  downloadResume(resume: any) {
    const byteCharacters = atob(resume.content);
    const byteNumbers = Array.from(byteCharacters, char => char.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: resume.fileType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = resume.fileName;
    a.click();
    URL.revokeObjectURL(url);
  }

  deleteResume(resumeId: string) {
    this.JobSeekerService.deleteResume(resumeId).subscribe({
      next: () => {
        this.refreshResumes();
      },
      error: (err) => console.error('Delete error:', err)
    });
  }

  refreshResumes() {
    const id = localStorage.getItem("Id");
    if (!id) return;

    this.JobSeekerService.getResumesByJobseekerId(id).subscribe({
      next: (res) => {
        this.resumes = res;
        if (this.resumes.length > 0) {
          this.selectedResumeId = this.resumes[0].id;
          this.selectedResume = this.resumes[0];
          this.resumeState.setHasResume(true);
        } else {
          this.selectedResumeId = '';
          this.selectedResume = null;
          this.resumeState.setHasResume(false);
        }
      },
      error: (err) => console.error('Error fetching resumes:', err)
    });
  }



  uploadResume(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const jobSeekerId = localStorage.getItem('Id');
    if (!jobSeekerId) return;

    const formData = new FormData();
    formData.append('JobSeekerId', jobSeekerId);
    formData.append('File', file);

    this.JobSeekerService.uploadResume(formData).subscribe({
      next: (data) => {
        this.resumeState.setHasResume(true);
        this.refreshResumes();
      },
      error: (err) => console.error('Upload error:', err)
    });
  }
}
