import { Component, OnInit } from '@angular/core';
import {Product} from "../../../model/product-model";
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";
import { Task } from 'src/app/model/task-model';
import {Route, Router} from "@angular/router";
import {SelectionModel} from "@angular/cdk/collections";
import {TaskStore} from "../../../store/task.store";

@Component({
  selector: 'app-ordering-cancelation',
  templateUrl: './ordering-cancelation.component.html',
  styleUrls: ['./ordering-cancelation.component.css']
})
export class OrderingCancelationComponent implements OnInit {
  columns: string[] = ['name', 'cancel'];
  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  selection = new SelectionModel<Product>(true, []);
  loading: boolean;

  private task: Task;
  dataSource: Product[];

  constructor(private authStore: AuthStore, private taskStore: TaskStore,
              private router: Router, private taskService: TaskService) {
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
