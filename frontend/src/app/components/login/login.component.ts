import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {FormBuilder, FormsModule, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  credentials: FormGroup;
  hide_password = true;


  constructor(private router: Router, fb: FormBuilder, private authenticationService: AuthenticationService) {
    this.credentials = fb.group({
      'username': ['', Validators.compose([Validators.required])],
      'password': ['', Validators.compose([Validators.required])],
    })
  }





  ngOnInit(): void {
  }

  login() : void {
    this.authenticationService.login(this.credentials.getRawValue().username, this.credentials.getRawValue().password);

    if (this.authenticationService.getCurrentUser() != null){
      this.router.navigate(["home"]).then(r => console);
    }else{
      alert("invalid credentials")
    }
  }


}
