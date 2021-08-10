import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  public isUserAuthenticated: boolean = false;
  isExpanded = false;

  constructor(private _authService: AuthService) {
  }
  ngOnInit(): void {
    this._authService.loginChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public login = () => {
    this._authService.login();
  }
  public logout = () => {
    this._authService.logout();
  }
}
