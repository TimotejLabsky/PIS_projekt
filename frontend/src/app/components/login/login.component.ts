import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {FormBuilder, FormsModule, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";
import {AuthStore} from "../../store/auth.store";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  credentials: FormGroup;
  hide_password = true;

  constructor(private router: Router, fb: FormBuilder, private authStore: AuthStore) {
    this.credentials = fb.group({
      'username': ['', Validators.compose([Validators.required])],
      'password': ['', Validators.compose([Validators.required])],
    })
  }

  ngOnInit(): void {
  }

  login() : void {
    this.authStore.login(this.credentials.getRawValue().username, this.credentials.getRawValue().password);

    this.router.navigate(['sales-optimalization'])
  }


}
