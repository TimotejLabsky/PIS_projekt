import {Injectable} from '@angular/core';
import {User} from "../model/user-model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor() {  }

  authenticate(userName: string, password: string): User{
    let user: User = null;

    if(userName == 'admin' && password == 'admin'){
      //TODO
      user = {userName: userName, email: '', task:null};

    }else {
      alert("Invalid credentials");
    }

    return user
  }


}
