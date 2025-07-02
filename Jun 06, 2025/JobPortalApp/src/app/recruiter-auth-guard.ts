import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";

@Injectable()
export class RecruiterAuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = localStorage.getItem("JwtToken");
    const role = localStorage.getItem("role");
    if (token && role === 'Recruiter') {
      return true;
    }
    this.router.navigate(["/recruiters"]);
    return false;
  }
}