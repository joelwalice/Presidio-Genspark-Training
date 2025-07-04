import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobSeekerService {
  http = inject(HttpClient);
  getJobSeekerById(Id: string) {
    const token = localStorage.getItem("JwtToken");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(`http://localhost:5039/api/jobseeker/${Id}`, { headers })
  }

  fetchJobSeekerByEmail(email: string) {
    return new Observable((observer) => {
      this.fetchAllJobSeeker().subscribe({
        next: (data: any[]) => {
          const matchedJobseeker = data.find(user => user.email === email);
          observer.next(matchedJobseeker);
          observer.complete();
        },
        error: (err) => {
          observer.error(err);
        }
      });
    });
  }
  deleteJobSeeker(id: string) {
    const token = localStorage.getItem("JwtToken");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete<any>(`http://localhost:5039/api/jobseeker/${id}`, { headers });
  }

  fetchAllJobSeeker() {
    const token = localStorage.getItem("JwtToken");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>("http://localhost:5039/api/jobseeker", { headers });
  }

  getResumesByJobseekerId(jobseekerId: string): Observable<any[]> {
    const token = localStorage.getItem('JwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    return this.http.get<any[]>(`http://localhost:5039/api/jobseeker/resumes/${jobseekerId}`, { headers });
  }

  uploadResume(formData: FormData) {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };

    return this.http.post<any>('http://localhost:5039/api/files/upload', formData, { headers });
  }

  deleteResume(resumeId: string): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.delete<any>(`http://localhost:5039/api/files/${resumeId}`, { headers });
  }

  getJobDetails(): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.get<any>("http://localhost:5039/api/jobs", { headers });
  }

  getJobDetailById(id: string): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.get<any>(`http://localhost:5039/api/jobs/${id}`, { headers });
  }

  getResumeById(resumeId: string): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.get<any>(`http://localhost:5039/api/files/resume/${resumeId}`, { headers });
  }

  getAppliedJobsByJobSeekerId(jobSeekerId: string): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.get<any>(`http://localhost:5039/api/JobApplication/applied/${jobSeekerId
      }`, { headers });
  }

  applyJobs(payload: {}): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    return this.http.post<any>(`http://localhost:5039/api/jobs/apply`, payload, { headers });
  }

  updateProfile(payload: any): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put<any>(`http://localhost:5039/api/jobseeker`, payload, { headers });
  }

  setDefaultResume(jobSeekerId: string, resumeId: string): Observable<any> {
    const token = localStorage.getItem('JwtToken');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.put(`http://localhost:5039/api/files/set-default`, {
      jobSeekerId,
      resumeId
    }, { headers });
  }
}
