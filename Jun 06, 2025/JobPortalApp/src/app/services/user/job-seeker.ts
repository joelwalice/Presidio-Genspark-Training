import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobSeekerService {
  http = inject(HttpClient);
  getJobSeekerById(Id : string){
    const token = sessionStorage.getItem("JwtToken");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    }); 
    return this.http.get<any>(`http://localhost:5039/api/jobseeker/${Id}`,{headers})
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

  fetchAllJobSeeker() {
    const token = sessionStorage.getItem("JwtToken");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>("http://localhost:5039/api/jobseeker", { headers });
  }

  getResumesByJobseekerId(jobseekerId: string): Observable<any[]> {
    const token = sessionStorage.getItem('JwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    return this.http.get<any[]>(`http://localhost:5039/api/jobseeker/resumes/${jobseekerId}`, { headers });
  }

  uploadResume(formData: FormData) {
    const token = sessionStorage.getItem('JwtToken');
    const headers = {
      Authorization: `Bearer ${token}`
    };

    return this.http.post<any>('http://localhost:5039/api/files/upload', formData, { headers });
  }
}
