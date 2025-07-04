import { Component, OnInit } from '@angular/core';
import { Navbar } from "../../components/navbar/navbar";
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { JobSeekerService } from '../../services/user/job-seeker';
import { ResumeStateService } from '../../services/user/resume-state-service';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-landing-page',
  imports: [Navbar, RouterOutlet, CommonModule, RouterLink, CurrencyPipe],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css'
})
export class LandingPage implements OnInit {
  fullName = 'J';
  email = 'j@g.com';
  phone = '9988776655';
  hasResume = false;
  appliedJobs: any[] = [];
  showToast = false;
  toastMessage = '';
  constructor(private JobSeekerService: JobSeekerService, private router: Router, private resumeState: ResumeStateService) {

  }
  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5039/notification') 
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      console.log('Connected to SignalR hub');
    }).catch(err => console.error('SignalR connection error:', err));

    connection.on('ReceiveNotification', (data: any) => {
      this.toastMessage = `New Job Posted: ${data.title} at ${data.companyName} - ₹${data.salary}`;
      this.showToast = true;
      this.fetchAppliedJobs(); 
      setTimeout(() => this.showToast = false, 5000);
      
    });
    const checkRole = () => {
      const role = localStorage.getItem("role");
      if (role === "JobSeeker") {
        this.router.navigate(['/jobseekers']);
      } else if (role === "Recruiter") {
        this.router.navigate(['/recruiters/home']);
      } else {
        this.router.navigate(['/login']);
      }
    };
    setTimeout(checkRole, 100);
    this.resumeState.appliedJobsChanged$.subscribe((changed) => {
      if (changed) this.fetchAppliedJobs();
    });

    this.fetchAppliedJobs();

    this.resumeState.hasResume$.subscribe((value) => {
      this.hasResume = value;
    });
    const getDataById = () => {
      const Id = localStorage.getItem("Id");
      if (Id) {
        this.JobSeekerService.getJobSeekerById(Id).subscribe({
          next: (data: any) => {
            this.fullName = data.name;
            this.email = data.email;
            this.phone = data.phoneNumber;
            const resumeStatus = !!data.defaultResumeId;
            this.hasResume = resumeStatus;
            this.resumeState.setHasResume(resumeStatus);
          }
        })
      }
      else {
        setTimeout(getDataById, 100);
      }
    }
    getDataById();

    const setJobDetails = () => {
      this.JobSeekerService.getJobDetails().subscribe({
        next: (data) => {
          this.jobs = data;
        },
        error: (err) => {
          console.log(err);
        }
      })
    }
    setJobDetails();
    const appliedJobs = () => {
      const id = localStorage.getItem("Id");
      if (id) {

        this.JobSeekerService.getAppliedJobsByJobSeekerId(id).subscribe({
          next: (data) => {
            this.appliedJobs = data;
          },
          error: (err) => {
            console.error('Error fetching applied jobs:', err);
          }
        })
      }
      else {
        setTimeout(appliedJobs, 100);
      }
    };

    appliedJobs();

  }
  get isOnJobSeekerLandingPage(): boolean {
    return this.router.url === '/jobseekers';
  }

  fetchAppliedJobs() {
    const id = localStorage.getItem("Id");
    if (id) {
      this.JobSeekerService.getAppliedJobsByJobSeekerId(id).subscribe({
        next: (data: any[]) => {
          this.appliedJobs = data;
        },
        error: (err) => console.error('Error fetching job details:', err)
      });
    }
  }
  jobs = [{
    id: '',
    title: "",
    description: "",
    location: "",
    companyName: "",
    salary: 0,
    requirements: "",
  }];
}
