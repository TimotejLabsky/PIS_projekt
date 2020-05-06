import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router, RouterModule} from "@angular/router";
import {AuthenticationService} from "../../services/authentication.service";
import {connectableObservableDescriptor} from "rxjs/internal/observable/ConnectableObservable";
import {TaskService} from "../../services/task.service";

@Component({
  selector: 'app-home',
  templateUrl: './sales-optimalization.component.html',
  styleUrls: ['./sales-optimalization.component.css']
})
export class SalesOptimalizationComponent implements OnInit {

  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private authenticationService: AuthenticationService,
              private taskService: TaskService) {

    let task = taskService.getTask(authenticationService.getCurrentUser());

    console.log(task);
    this.router.navigate([task.taskType], {relativeTo: this.activatedRoute});

  }

  ngOnInit(): void {

  }


}
