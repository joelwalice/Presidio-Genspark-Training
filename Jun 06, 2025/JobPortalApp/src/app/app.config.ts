import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { UserLoginService } from './services/user/UserLoginService';
import { UserRegisterService } from './services/user/UserRegisterService';
import { JobSeekerService } from './services/user/job-seeker';
import { AuthGuard } from './auth-guard';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    UserLoginService,
    UserRegisterService,
    JobSeekerService,
    AuthGuard
  ]
};
