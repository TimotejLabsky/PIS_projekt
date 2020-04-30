import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {FormBuilder, FormsModule, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  credentials: FormGroup;
  hide_password = true;


  constructor(private router: Router, fb: FormBuilder) {
    this.credentials = fb.group({
      'username': ['', Validators.compose([Validators.required])],
      'password': ['', Validators.compose([Validators.required])],
    })
  }





  ngOnInit(): void {
  }

  /*login() : void {
    if(this.credentials.getRawValue() == 'admin' && this.credentials.getRawValue() == 'admin'){
      this.router.navigate(["user"]);
    }else {
      alert("Invalid credentials");
    }
  }*/

}
