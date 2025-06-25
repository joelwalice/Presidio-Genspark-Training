import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { HomePage } from './home-page/home-page';
import { LandingPage } from './jobseekers/landing-page/landing-page';
import { ResumePage } from './jobseekers/resume-page/resume-page';
import { ProfilePage } from './jobseekers/profile-page/profile-page';
import { AuthGuard } from './auth-guard';

export const routes: Routes = [
    { path: '', component: HomePage },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    {
        path: 'jobseekers', component: LandingPage, canActivate : [AuthGuard], children: [
            { path: 'profile', component: ProfilePage }
        ]
    }
];
