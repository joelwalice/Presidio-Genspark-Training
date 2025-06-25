import { Component, OnInit } from '@angular/core';
import { JobSeekerService } from '../../services/user/job-seeker';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-resume-page',
  imports: [CommonModule],
  templateUrl: './resume-page.html',
  styleUrl: './resume-page.css'
})
export class ResumePage implements OnInit {
  resume: any = null;

  constructor(private JobSeekerService: JobSeekerService, private http: HttpClient, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    const getResumeFn = () => {
      const Id = sessionStorage.getItem("Id");
      if (Id) {
        this.JobSeekerService.getResumesByJobseekerId(Id).subscribe({
          next: (resumes) => {
            if (resumes.length > 0) {
              this.resume = resumes[0];
            }
          },
          error: (err) => console.error('Error fetching resumes:', err)
        })
      }
      else{
        setTimeout(getResumeFn, 100);
      }
    }
    getResumeFn();
  }

  downloadResume() {
    const byteCharacters = atob(this.resume.content);
    const byteNumbers = Array.from(byteCharacters, char => char.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: this.resume.fileType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = this.resume.fileName;
    a.click();
    URL.revokeObjectURL(url);
  }

  deleteResume(resumeId: string) {
    this.http.delete(`/api/resumes/${resumeId}`).subscribe({
      next: () => this.resume = null,
      error: err => console.error('Error deleting resume:', err)
    });
  }

  uploadResume(event: Event) {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];

  if (!file) return;

  const token = sessionStorage.getItem('JwtToken');
  const jobSeekerId = sessionStorage.getItem('Id');

  if (!jobSeekerId) {
    console.error('JobSeekerId not found in token');
    return;
  }

  const formData = new FormData();
  formData.append('JobSeekerId', jobSeekerId);
  formData.append('File', file);

  this.JobSeekerService.uploadResume(formData).subscribe({
    next: (data) => {
      console.log('Upload success:', data);
      this.resume = data;
    },
    error: (err) => {
      console.error('Upload error:', err);
    }
  });
}

}
