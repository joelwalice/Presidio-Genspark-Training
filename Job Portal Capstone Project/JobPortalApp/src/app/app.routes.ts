import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { HomePage } from './home-page/home-page';
import { LandingPage } from './jobseekers/landing-page/landing-page';
import { ResumePage } from './jobseekers/resume-page/resume-page';
import { ProfilePage } from './jobseekers/profile-page/profile-page';
import { JobSeekerAuthGuard } from './jobseeker-auth-guard';
import { RecruiterLogin } from './recruiters/login/login';
import { Explore } from './jobseekers/explore/explore';
import { ApplyJobs } from './jobseekers/apply-jobs/apply-jobs';
import { RecruiterRegister } from './recruiters/register/register';
import { RecruiterLandingPage } from './recruiters/recruiter-landing-page/recruiter-landing-page';
import { RecruiterAuthGuard } from './recruiter-auth-guard';
import { RecruiterCompany } from './recruiters/recruiter-company/recruiter-company';
import { RecruiterJobs } from './recruiters/recruiter-jobs/recruiter-jobs';
import { RecruiterApplicants } from './recruiters/recruiter-applicants/recruiter-applicants';
import { RecruiterProfilePage } from './recruiters/recruiter-profile-page/recruiter-profile-page';


export const routes: Routes = [
    { path: '', component: HomePage },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    {
        path: 'jobseekers', component: LandingPage, canActivate: [JobSeekerAuthGuard], children: [
            { path: 'profile', component: ProfilePage },
            { path: 'explore', component: Explore },
            { path: 'apply/:id', component: ApplyJobs }
        ]
    },
    {
        path: 'recruiters', component: RecruiterLogin, children: [
            { path: 'register', component: RecruiterRegister },
            {
                path: 'home', component: RecruiterLandingPage, canActivate: [RecruiterAuthGuard], children: [
                    {
                        path: 'company', component : RecruiterCompany
                    },
                    {
                        path : 'jobs', component : RecruiterJobs
                    },
                    {
                        path : 'profile', component : RecruiterProfilePage
                    },
                    {
                        path : 'applicants/:jobId', component : RecruiterApplicants
                    }
                ]
            }
        ]
    }
];
