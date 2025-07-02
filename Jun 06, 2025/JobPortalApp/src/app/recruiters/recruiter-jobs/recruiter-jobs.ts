import { Component, OnInit, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin, map } from 'rxjs';

@Component({
  selector: 'app-recruiter-jobs',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './recruiter-jobs.html',
  styleUrl: './recruiter-jobs.css'

})
export class RecruiterJobs implements OnInit {
  jobs: any[] = [];
  jobForm!: FormGroup;
  formVisible: boolean = false;
  recruiterId: string | null = null;
  companyId: string | null = null;
  editMode: boolean = false;
  JobId: string | null = null;

  constructor(private recruiterService: RecruiterService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.recruiterId = localStorage.getItem('Id');
    this.buildForm();
    if (this.recruiterId) {
      this.loadJobs(this.recruiterId);
    }
    const getCompanyDetailsByID = () => {
      if (this.recruiterId) {
        this.recruiterService.getCompaniesByRecruiterId(this.recruiterId!).subscribe({
          next: (companies) => {
            this.companyId = Array.isArray(companies) && companies.length > 0 ? companies[0].id : null;
            this.jobForm.patchValue({
              companyName: companies[0]?.name || ''
            });
          },
          error: (err) => console.error('Error fetching company details:', err)
        });
      }
      else {
        setTimeout(getCompanyDetailsByID, 1000);
      }

    }
    getCompanyDetailsByID();
  }
  toggleEditMode(job: any) {
    this.editMode = true;
    this.formVisible = true;
    this.jobForm.patchValue({
      title: job.title,
      location: job.location,
      requirements: job.requirements,
      salary: job.salary,
      expiryDate: this.formatDateToInput(job.expiryDate),
      companyName: job.companyName,
      description: job.description
    });

    this.companyId = job.companyId;
    this.jobForm.addControl('id', this.fb.control(job.id));
    this.jobForm.get('companyName')?.disable();
  }

  formatDateToInput(dateValue: string | Date): string {
    const date = new Date(dateValue);
    const year = date.getFullYear();
    const month = (`0${date.getMonth() + 1}`).slice(-2);
    const day = (`0${date.getDate()}`).slice(-2);
    return `${year}-${month}-${day}`;
  }


  updateJob() {
    const values = this.jobForm.getRawValue();
    const payload = {
      id: values.id,
      title: values.title,
      location: values.location,
      requirements: values.requirements,
      salary: values.salary,
      expiryDate: values.expiryDate ? `${values.expiryDate}T00:00:00Z` : null,
      recruiterId: this.recruiterId,
      companyId: this.companyId,
      companyName: values.companyName,
      description: values.description
    };

    this.recruiterService.updateJobDetails(payload).subscribe({
      next: () => {
        this.loadJobs(this.recruiterId!);
        this.cancelJobForm();
      },
      error: (err) => console.error('Error updating job:', err)
    });
  }
  deleteJob(jobId: string) {
    if (confirm('Are you sure you want to delete this job?')) {
      this.recruiterService.deleteJob(jobId).subscribe({
        next: () => {
          this.loadJobs(this.recruiterId!);
        },
        error: (err) => console.error('Error deleting job:', err)
      });
    }
  }

  buildForm() {
    this.jobForm = this.fb.group({
      title: ['', Validators.required],
      location: ['', Validators.required],
      requirements: ['', Validators.required],
      salary: [null, [Validators.required, Validators.min(0)]],
      expiryDate: ['', Validators.required],
      companyName: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  showJobForm() {
    this.editMode = false;
    this.jobForm.reset();
    this.formVisible = true;
    const companyName = this.jobForm.get('companyName');
    if (this.companyId && companyName) {
      this.recruiterService.getCompanyById(this.companyId).subscribe({
        next: (company) => {
          companyName.patchValue(company.name);
          companyName.disable();
        },
        error: (err) => console.error('Error fetching company for form:', err)
      });
    }
  }

  cancelJobForm() {
    this.formVisible = false;
    this.jobForm.reset();
  }

  submitJob() {
    if (this.jobForm.invalid || !this.recruiterId || !this.companyId) return;

    const jobPayload = {
      ...this.jobForm.getRawValue(),
      expiryDate: this.jobForm.value.expiryDate
        ? `${this.jobForm.value.expiryDate}T00:00:00`
        : '',
      recruiterId: this.recruiterId,
      companyId: this.companyId
    };
    this.recruiterService.addJob(jobPayload).subscribe({
      next: () => {
        this.cancelJobForm();
        this.loadJobs(this.recruiterId!);
      },
      error: (err) => console.error('Error creating job:', err)
    });
  }

  loadJobs(recruiterId: string) {
    this.recruiterService.getAllJobs().subscribe({
      next: (allJobs: any[]) => {
        const recruiterJobs = allJobs.filter(job => job.recruiterId === recruiterId);
        const fetches = recruiterJobs.map(job =>
          this.recruiterService.getJobApplicationsByJobId(job.id).pipe(
            map(applicants => ({
              ...job,
              applicantCount: applicants.length || 0
            }))
          )
        );

        forkJoin(fetches).subscribe({
          next: (result) => {
            this.jobs = result;
          },
          error: (err) => console.error('Error fetching job applicants count', err)
        });
      },
      error: (err) => console.error('Error fetching jobs', err)
    });
  }
}
