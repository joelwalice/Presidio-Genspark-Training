import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate,  Router, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate{

  constructor(private router:Router){}

  canActivate(route:ActivatedRouteSnapshot, state:RouterStateSnapshot):boolean{
    const isAuthenticated = sessionStorage.getItem("JwtToken")?true:false;
    if(!isAuthenticated)
    {
      this.router.navigate(["login"]);
      return false;
    } 
    return true;
  }  
  
}
