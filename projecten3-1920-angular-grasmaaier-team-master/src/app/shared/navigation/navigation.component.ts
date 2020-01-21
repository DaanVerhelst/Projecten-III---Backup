import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Modes } from './navigations.modes';
import { AuthenticationService } from 'src/app/user/authentication.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  @Input() navMode: Modes;
  protected navStyle: string;
  protected navTitleStyle: string;

  // tslint:disable-next-line: variable-name
  constructor(private _route: Router, private authService: AuthenticationService) { }

  ngOnInit() {
    if (this.navMode === Modes.Login) {
      this.navStyle = 'login-nav';
      this.navTitleStyle = 'login-nav-title';
    } else {
      this.navStyle = 'default-nav';
      this.navTitleStyle = '';
    }
  }

  logout() {
      this.authService.logout();
  }
}
