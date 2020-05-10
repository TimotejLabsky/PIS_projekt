import {Injectable} from '@angular/core';
import {User} from "../model/user-model";
import {Task, TaskType} from "../model/task-model";
import {Observable, of} from "rxjs";
import {base_endpoint} from "../url.constatns";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private endpoint: string = base_endpoint + '/tasks';

  constructor(private httpClient: HttpClient) { }

  getTask(user: User): Observable<Task>{
    //console.log("TASK_SERVICE: ", this.endpoint + '/next')
    return this.httpClient.get<Task>(this.endpoint + '/next')
  }

  fulfillTask(task: Task): Observable<any>{
    return this.httpClient.post<any>(this.endpoint + '/fulfill', task);
  }

}
