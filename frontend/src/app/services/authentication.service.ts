import {Injectable} from '@angular/core';
import {User} from "../model/user-model";
import {HttpClient} from "@angular/common/http";
import {base_endpoint} from "../url.constatns";
import {Observable, of} from "rxjs";
import {stringify} from "querystring";
import {delay, tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private endpoint: string = base_endpoint + '/auth';

  constructor(private httpClient: HttpClient) {  }

  authenticate(userName: string, password: string): Observable<User>{
    /*
    {
      userName: string
      password: string
    }
    */
    let body = { userName: userName, password: password};
    /**
     *  return this.httpClient.post<User>(this.endpoint, body);
     */

    let user: User = null;

    if(userName == 'admin' && password == 'admin') {
      user = {userName: userName, task: null};
    }

    return of(user).pipe(
      delay(2000),
      tap((user: User) => console.log(`authenticate =${body.userName}`))
    )
  }


}
