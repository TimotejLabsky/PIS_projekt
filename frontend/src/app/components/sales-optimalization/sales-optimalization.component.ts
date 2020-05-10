import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";
import {Task, TaskType} from 'src/app/model/task-model';
import {TaskStore} from "../../store/task.store";
import {interval, Subscription} from "rxjs";

@Component({
  selector: 'app-home',
  templateUrl: './sales-optimalization.component.html',
  styleUrls: ['./sales-optimalization.component.css']
})
export class SalesOptimalizationComponent implements OnInit {
  loading: boolean;
  private task: Task;

  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private authStore: AuthStore, private taskService: TaskService,
              private taskStore: TaskStore) {

    this.loading = true;

    this.taskStore.loadTask()

    this.taskStore.$currentTask.subscribe(
      task => this.initComplete(task),
      error => console.error(error),
    )

  }

  ngOnInit(): void {
  }



  private initComplete(task: Task){
    this.task = task;
    this.loading = false
    try {
      this.router.navigate([task.taskType], {relativeTo: this.activatedRoute});

    }catch (e) {
      this.router.navigate([TaskType.nothing], {relativeTo: this.activatedRoute});

    }
  }


}
