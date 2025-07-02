import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecruiterService {
  fetchRecruiterByEmail(email: string) {
    return new Observable((observer) => {
      this.getAllRecruiters().subscribe({
        next: (data: any[]) => {
          const matchedRecruiter = data.find(user => user.email === email);
          observer.next(matchedRecruiter);
          observer.complete();
        },
        error: (err) => {
          observer.error(err);
        }
      });
    });
  }
  updateProfile(editable: { name: string; email: string; phoneNumber: string; address: string; id: string; password: string; }) {
    const token = localStorage.getItem('JwtToken');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put<any>(`http://localhost:5039/api/jobseeker`, editable, { headers });
  }

  private apiUrl = 'http://localhost:5039/api';
  private token = localStorage.getItem('JwtToken');

  private get headers() {
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${this.token}`
      })
    };
  }

  constructor(private http: HttpClient) { }

  fetchJobSeekerByEmail(email: string) {
    return new Observable((observer) => {
      this.getAllRecruiters().subscribe({
        next: (data: any[]) => {
          const matchedRecruiter = data.find(user => user.email === email);
          observer.next(matchedRecruiter);
          observer.complete();
        },
        error: (err) => {
          observer.error(err);
        }
      });
    });
  }

  getAllRecruiters(): Observable<any> {
    return this.http.get(`${this.apiUrl}/recruiter`, this.headers);
  }

  getRecruiterById(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/recruiter/${id}`, this.headers);
  }

  addRecruiter(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/recruiter`, payload, this.headers);
  }

  updateRecruiter(payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/recruiter`, payload, this.headers);
  }

  deleteRecruiter(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/recruiter/${id}`, this.headers);
  }

  getAllJobs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/jobs`, this.headers);
  }

  getJobById(jobId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/jobs/${jobId}`, this.headers);
  }

  addJob(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/jobs`, payload, this.headers);
  }

  getAllCompanies(): Observable<any> {
    return this.http.get(`${this.apiUrl}/company`, this.headers);
  }

  getCompanyById(companyId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/company/${companyId}`, this.headers);
  }

  addCompany(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/company`, payload, this.headers);
  }

  updateCompany(payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/company`, payload, this.headers);
  }

  deleteCompany(companyId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/company/${companyId}`, this.headers);
  }

  getCompanyRecruiters(companyId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/company/${companyId}/recruiters`, this.headers);
  }

  getCompanyJobs(companyId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/company/${companyId}/jobs`, this.headers);
  }

  loginRecruiter(payload: { email: string; password: string; role: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Authentication/auth/login`, payload);
  }

  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/authentication/auth/logout`, {}, this.headers);
  }

  getCurrentUser(): Observable<any> {
    return this.http.get(`${this.apiUrl}/authentication/auth/me`, this.headers);
  }

  sendNotification(message: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Notification/send`, `"${message}"`, this.headers);
  }

  getCompaniesByRecruiterId(recruiterId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/company/recruiter/${recruiterId}`);
  }

  getJobApplicationsByJobId(jobId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/jobApplication/job/${jobId}`, this.headers);
  }

  updateApplicationStatus(id: string, status: number): Observable<any> {
    const payload = { jobStatus: status };
    return this.http.put(`${this.apiUrl}/jobApplication/${id}/status`, payload, this.headers);
  }

  deleteJob(jobId: string) {
    return this.http.delete(`${this.apiUrl}/jobs/${jobId}`, this.headers);
  }

  updateJobDetails(payload: any): Observable<any> {
    console.log('Updating job details:', payload);
    if (!payload.id) {
      console.error('Job ID is required for updating job details');
      return new Observable(observer => {
        observer.error('Job ID is required for updating job details');
        observer.complete();
      });
    }
    return this.http.put(`${this.apiUrl}/jobs/${payload.id}`, payload, this.headers);
  }
}
