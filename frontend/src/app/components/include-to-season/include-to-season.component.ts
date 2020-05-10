import { Component, OnInit } from '@angular/core';
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import { Task } from 'src/app/model/task-model';

@Component({
  selector: 'app-include-to-season',
  templateUrl: './include-to-season.component.html',
  styleUrls: ['./include-to-season.component.css']
})
export class IncludeToSeasonComponent implements OnInit {
  actual_season: any;
  dataSource: Observable<Task>;
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    this.dataSource = this.taskService.getTask(authStore.getCurrentUser());
  }

  ngOnInit(): void {
  }

}
