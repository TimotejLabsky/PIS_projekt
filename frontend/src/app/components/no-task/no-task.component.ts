import { Component, OnInit } from '@angular/core';
import {interval, Subscription} from "rxjs";
import {TaskStore} from "../../store/task.store";

@Component({
  selector: 'app-no-task',
  templateUrl: './no-task.component.html',
  styleUrls: ['./no-task.component.css']
})
export class NoTaskComponent implements OnInit {
  private updateSubscription: Subscription;

  constructor(private taskStore: TaskStore) { }

  ngOnInit(): void {
    /*this.updateSubscription = interval(2000).subscribe(
      (val) => { this.taskStore.loadTask()
      });*/
  }

}
