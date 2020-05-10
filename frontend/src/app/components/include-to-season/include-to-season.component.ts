import { Component, OnInit } from '@angular/core';
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";
import {Product} from "../../model/product-model";

@Component({
  selector: 'app-include-to-season',
  templateUrl: './include-to-season.component.html',
  styleUrls: ['./include-to-season.component.css']
})
export class IncludeToSeasonComponent implements OnInit {
  actual_season: any;
  dataSource: Product[];
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    this.dataSource = this.taskService.getTask(authStore.getCurrentUser()).products;
  }

  ngOnInit(): void {
  }

}
