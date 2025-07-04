import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recruiter-company',
  imports : [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './recruiter-company.html',
  styleUrls: ['./recruiter-company.css']
})
export class RecruiterCompany implements OnInit {
  recruiterId = localStorage.getItem("Id") || '';
  companies: any[] = [];
  formVisible = false;
  editMode = false;
  companyForm: FormGroup;
  selectedCompanyId: string | null = null;

  constructor(private fb: FormBuilder, private recruiterService: RecruiterService) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      email: [''],
      phoneNumber: [''],
      establishedDate : ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.fetchCompanies();
  }

  fetchCompanies() {
    this.recruiterService.getCompaniesByRecruiterId(this.recruiterId).subscribe({
      next: (res) =>{
        this.companies = res;
      } ,
      error: (err) => console.error('Failed to load companies', err)
    });
  }

  showForm(company: any = null) {
  this.editMode = !!company;
  this.formVisible = true;

  if (company) {
    const formattedDate = company.establishedDate?.split('T')[0]; // Keep only YYYY-MM-DD
    this.selectedCompanyId = company.id;
    this.companyForm.patchValue({ ...company, establishedDate: formattedDate });
  } else {
    this.companyForm.reset();
  }
}

submitCompany() {
  if (this.companyForm.invalid) return;

  const rawDate = this.companyForm.value.establishedDate;
  const fullDateTime = rawDate ? new Date(`${rawDate}T00:00:00Z`) : null;

  const payload = {
    ...this.companyForm.value,
    establishedDate: fullDateTime,
    id: this.selectedCompanyId || undefined,
  };

  const request = this.editMode
    ? this.recruiterService.updateCompany( payload)
    : this.recruiterService.addCompany(payload);

  request.subscribe({
    next: () => {
      this.fetchCompanies();
      this.formVisible = false;
      this.companyForm.reset();
    },
    error: (err) => console.error('Save failed', err)
  });
}

  cancel() {
    this.formVisible = false;
    this.companyForm.reset();
    this.selectedCompanyId = null;
    this.editMode = false;
  }
}
