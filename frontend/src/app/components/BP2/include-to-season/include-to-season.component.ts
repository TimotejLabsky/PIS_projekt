import { Component, OnInit } from '@angular/core';
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";
import {Product} from "../../../model/product-model";
import {SelectionModel} from "@angular/cdk/collections";
import { Task } from 'src/app/model/task-model';
import {TaskStore} from "../../../store/task.store";


@Component({
  selector: 'app-include-to-season',
  templateUrl: './include-to-season.component.html',
  styleUrls: ['./include-to-season.component.css']
})
export class IncludeToSeasonComponent implements OnInit {
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];

  actual_season: any;
  dataSource: Product[];
  private task: Task;
  loading: boolean;
  selection = new SelectionModel<Product>(true, []);


  constructor(private taskStore: TaskStore, private authStore: AuthStore) {
    this.loading = true;
    this.initComplete(this.taskStore.getTask());
  }

  private initComplete(task: Task){
    this.task = task;
    this.dataSource = this.task.products;
    this.loading = false;
  }
  ngOnInit(): void {
  }

  onSubmit(){
    this.task.products = this.dataSource;

    this.task.products.forEach(product => (this.selection.selected.filter((item) => item == product)).length > 0
      ? product.cancel_ordering = true : product.cancel_ordering = false);

    this.taskService.fulfillTask(this.task).subscribe(
      value => console.log(value),
      error => console.error(error),
      () => this.router.navigate(['sales-optimalization'])
    );
  }
}
