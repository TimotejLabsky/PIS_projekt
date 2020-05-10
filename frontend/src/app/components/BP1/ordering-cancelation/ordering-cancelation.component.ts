import { Component, OnInit } from '@angular/core';
import {Product} from "../../../model/product-model";
import {Observable} from "rxjs";
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";
import { Task } from 'src/app/model/task-model';
import {TaskLogger} from "protractor/built/taskLogger";
import {Route, Router} from "@angular/router";
import {SelectionModel} from "@angular/cdk/collections";

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

  private task: Task;
  dataSource: Product[];
  loading: boolean;

  constructor(private taskService: TaskService, private authStore: AuthStore,
              private router: Router) {
    this.loading = true;
    this.taskService.getTask(authStore.getCurrentUser()).subscribe(
      task => this.task = task,
      err => console.error(err),
      () => this.initComplete()
    );
  }

  private initComplete(){
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
