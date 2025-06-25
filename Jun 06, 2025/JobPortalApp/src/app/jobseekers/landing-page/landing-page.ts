import { Component, OnInit } from '@angular/core';
import { Navbar } from "../../components/navbar/navbar";
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { JobSeekerService } from '../../services/user/job-seeker';

@Component({
  selector: 'app-landing-page',
  imports: [Navbar, RouterOutlet, CommonModule, RouterLink, CurrencyPipe],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css'
})
export class LandingPage implements OnInit {
  fullName = 'Joel Wilson';
  email = 'joel@example.com';
  phone = '+91 98765 43210';
  hasResume = true;
  constructor(private JobSeekerService: JobSeekerService, private router: Router) {

  }
  ngOnInit(): void {
    const getDataById = () => {
      const Id = sessionStorage.getItem("Id");
      if (Id) {
        this.JobSeekerService.getJobSeekerById(Id).subscribe({
          next: (data: any) => {
            this.fullName = data.name;
            this.email = data.email;
            this.phone = data.phoneNumber;
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
  }
  get isOnJobSeekerLandingPage(): boolean {
    return this.router.url === '/jobseekers';
  }
  jobs = [{
    title: "xUnit Tester",
    description: "Personnel to be knowledged in xUnit Testing, Application Testing",
    location: "USA",
    companyName: "Presidio Pvt Ltd",
    salary: 600000,
    requirements: "Unit Testing in ASP.Net",
  }];
}
