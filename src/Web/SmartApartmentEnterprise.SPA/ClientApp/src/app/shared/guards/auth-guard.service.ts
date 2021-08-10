import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private _authService: AuthService, private _router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.checkIsUserAuthenticated();
  }
  private checkIsUserAuthenticated() {
    return this._authService.isAuthenticated()
      .then(res => {
        return res ? true : this.goToLogin();
      })
  }
  private goToLogin() {
    this._authService.login();
    return false;
  }
}
