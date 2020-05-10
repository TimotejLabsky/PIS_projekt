import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, throwError} from "rxjs";
import {User} from "../model/user-model";
import {AuthenticationService} from "../services/authentication.service";
import {LoginComponent} from "../components/login/login.component";

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

  public login(userName: string, password: string) {
    this.authenticationService.authenticate(userName, password).subscribe(
      user => this.setUser(user),
    );
  }

  private setUser(user: User){
    localStorage.setItem('currentUser', JSON.stringify(user));
    this._currentUserSubject.next(user);
  }

  public logout(){
    localStorage.removeItem('currentUser');
    this._currentUserSubject.next(null);
  }

  public getCurrentUser(): User{
    return this._currentUserSubject.value
  }

}
