import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router, RouterModule} from "@angular/router";
import {AuthenticationService} from "../../services/authentication.service";
import {connectableObservableDescriptor} from "rxjs/internal/observable/ConnectableObservable";
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";
import { Task } from 'src/app/model/task-model';

@Component({
  selector: 'app-home',
  templateUrl: './sales-optimalization.component.html',
  styleUrls: ['./sales-optimalization.component.css']
})
export class SalesOptimalizationComponent implements OnInit {

  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private authStore: AuthStore,
              private taskService: TaskService) {

    let task = null;

    taskService.getTask(authStore.getCurrentUser()).subscribe(
      task => this.router.navigate([task.taskType], {relativeTo: this.activatedRoute})
    );


  }

  ngOnInit(): void {

  }


}
