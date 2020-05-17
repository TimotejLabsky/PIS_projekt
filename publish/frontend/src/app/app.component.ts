import { Component } from '@angular/core';
import {User} from "./model/user-model";
import {Router} from "@angular/router";
import {AuthenticationService} from "./services/authentication.service";
import {AuthStore} from "./store/auth.store";

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
    private authStore: AuthStore) {
    this.authStore.$currentUser.subscribe(x => this.handleUserChange(x));
  }

  handleUserChange(user: User){
    if (user == null) {
      this.router.navigate([""]).then();
    }
  }
  logout(){
    this.authStore.logout();
  }

}
