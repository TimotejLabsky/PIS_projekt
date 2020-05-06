import { Injectable } from '@angular/core';
import {User} from "../model/user-model";
import {Task} from "../model/task-model";

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor() { }

  getTask(user: User): Task{
    return
  }
}
