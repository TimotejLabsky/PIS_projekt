import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import {AuthenticationService} from "./services/authentication.service";
import {AuthStore} from "./store/auth.store";


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router,
              private authStore: AuthStore) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.authStore.getCurrentUser();

    if (currentUser) {
      // logged in so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    //{ queryParams: { returnUrl: state.url } }
    this.router.navigate(['/login']);
    return false;
  }
}
