import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {User} from "../model/user-model";
import {TaskType} from "../model/task-model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private _currentUserSubject: BehaviorSubject<User>;
  public readonly $currentUser: Observable<User>;

  constructor() {
    this._currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.$currentUser = this._currentUserSubject.asObservable();
  }

  public getCurrentUser(): User{
    return this._currentUserSubject.value
  }

  login(userName: string, password: string){
    if(userName == 'admin' && password == 'admin'){
      //TODO
      let user = {userName: userName, email: '', task:{taskType: TaskType.advertisement_picking}}
      this._currentUserSubject.next(user);
      localStorage.setItem('currentUser', JSON.stringify(user));

    }else {
      alert("Invalid credentials");
    }
  }

  logout(){
    localStorage.removeItem('currentUser');
    this._currentUserSubject.next(null);
  }
}
