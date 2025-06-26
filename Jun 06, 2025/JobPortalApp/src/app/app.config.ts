import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { UserLoginService } from './services/user/UserLoginService';
import { UserRegisterService } from './services/user/UserRegisterService';
import { JobSeekerService } from './services/user/job-seeker';
import { AuthGuard } from './auth-guard';
import { AuthInterceptor } from './auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([AuthInterceptor])
    ),
    UserLoginService,
    UserRegisterService,
    JobSeekerService,
    AuthGuard
  ]
};
