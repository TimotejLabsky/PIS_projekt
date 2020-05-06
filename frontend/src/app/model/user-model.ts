import {Task} from "./task-model"

export interface User {
  userName: string;
  email: string;
  task: Task;
}
