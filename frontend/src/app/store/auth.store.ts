import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {User} from "../model/user-model";
import {AuthenticationService} from "../services/authentication.service";

@Injectable({
  providedIn: 'root'
})
export class AuthStore {
  private _currentUserSubject: BehaviorSubject<User>;
  public readonly $currentUser: Observable<User>;

  constructor(private authenticationService: AuthenticationService) {
    this._currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.$currentUser = this._currentUserSubject.asObservable();
  }

  public login(userName: string, password: string): boolean{

    let user: User = this.authenticationService.authenticate(userName, password);
    localStorage.setItem('currentUser', JSON.stringify(user));
    this._currentUserSubject.next(user);
    return (user != null);
  }

  public logout(){
    localStorage.removeItem('currentUser');
    this._currentUserSubject.next(null);
  }

  public getCurrentUser(): User{
    return this._currentUserSubject.value
  }

}
