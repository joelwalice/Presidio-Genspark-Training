import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate,  Router, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class JobSeekerAuthGuard implements CanActivate{

  constructor(private router:Router){}

  canActivate(route:ActivatedRouteSnapshot, state:RouterStateSnapshot):boolean{
    const isAuthenticated = localStorage.getItem("JwtToken");
    const role = localStorage.getItem("role");
    if(!isAuthenticated || role !== 'JobSeeker')
    {
      this.router.navigate(["login"]);
      return false;
    }
    return true;
  }  
  
}
