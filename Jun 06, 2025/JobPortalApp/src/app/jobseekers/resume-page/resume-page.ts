import { Component, OnInit } from '@angular/core';
import { JobSeekerService } from '../../services/user/job-seeker';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

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

  constructor(private JobSeekerService: JobSeekerService, private http: HttpClient, private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    const getResumeFn = () => {
      const id = sessionStorage.getItem("Id");
      if (id) {
        this.JobSeekerService.getResumesByJobseekerId(id).subscribe({
          next: (res) => {
            this.resumes = res;
            if (this.resumes.length > 0) {
              this.selectedResumeId = this.resumes[0].id;
              this.selectedResume = this.resumes[0];
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
    this.http.delete(`/api/resumes/${resumeId}`).subscribe({
      next: () => {
        this.resumes = this.resumes.filter(r => r.id !== resumeId);
        if (this.resumes.length > 0) {
          this.selectedResumeId = this.resumes[0].id;
          this.selectedResume = this.resumes[0];
        } else {
          this.selectedResumeId = '';
          this.selectedResume = null;
        }
      },
      error: err => console.error('Error deleting resume:', err)
    });
  }

  uploadResume(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const jobSeekerId = sessionStorage.getItem('Id');
    if (!jobSeekerId) return;

    const formData = new FormData();
    formData.append('JobSeekerId', jobSeekerId);
    formData.append('File', file);

    this.JobSeekerService.uploadResume(formData).subscribe({
      next: (data) => {
        this.resumes.push(data);
        this.selectedResumeId = data.id;
        this.selectedResume = data;
      },
      error: (err) => console.error('Upload error:', err)
    });
  }
}
