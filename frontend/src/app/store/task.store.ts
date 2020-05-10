import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {User} from "../model/user-model";
import {TaskService} from "../services/task.service";
import {AuthStore} from "./auth.store";
import {Task} from "../model/task-model";

@Injectable({
  providedIn: 'root'
})
export class TaskStore {
  private _currentTaskSubject: BehaviorSubject<Task>;
  public readonly $currentTask: Observable<Task>;

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    this._currentTaskSubject = new BehaviorSubject<Task>(null);
    this.$currentTask = this._currentTaskSubject.asObservable();
  }

  public getTask(): Task{
    return this._currentTaskSubject.value;
  }

  public loadTask(){
    this.taskService.getTask(this.authStore.getCurrentUser()).subscribe(
      task => this._currentTaskSubject.next(task),
      error => console.error(error)
    )
  }
}
