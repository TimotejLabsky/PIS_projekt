import { Component } from '@angular/core';
import {User} from "./model/user-model";
import {Router} from "@angular/router";
import {AuthenticationService} from "./services/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'frontend';
  currentUser: User;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) {
    this.authenticationService.$currentUser.subscribe(x => this.handleUserChange(x));
  }

  handleUserChange(user: User){
    if (user == null) {
      this.router.navigate([""]).then();
    }
  }
  logout(){
    this.authenticationService.logout();
  }

}
