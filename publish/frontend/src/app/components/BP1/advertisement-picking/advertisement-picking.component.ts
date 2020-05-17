import {Component, Input, OnInit} from '@angular/core';
import {Product} from "../../../model/product-model";
import { MatTableDataSource } from "@angular/material/table";
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";
import {combineAll} from "rxjs/operators";
import {Router} from "@angular/router";
import { Task } from 'src/app/model/task-model';
import {TaskStore} from "../../../store/task.store";
import {SelectionModel} from "@angular/cdk/collections";


@Component({
  selector: 'app-advertisement-picking',
  templateUrl: './advertisement-picking.component.html',
  styleUrls: ['./advertisement-picking.component.css']
})
export class AdvertisementPickingComponent implements OnInit {
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include']; //TODO
  dataSource: MatTableDataSource<Product>
  actual_season: string = "20.20.2020-25.20.2020";
  loading: boolean;

  selection = new SelectionModel<Product>(true, []);

  private task: Task = null;

  constructor(private authStore: AuthStore, private taskService: TaskService,
              private router: Router, private taskStore: TaskStore) {

    this.loading = true;
    this.initComplete(this.taskStore.getTask());

  }

  private initComplete(task: Task){
    this.task = task;
    this.dataSource = new MatTableDataSource(task.products);
    this.loading = false;
  }

  ngOnInit(): void {

  }

  applyFilter($event: KeyboardEvent) {
    const filterValue = ($event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onSubmit() {
    this.task.products = this.dataSource.data;

    this.task.products.forEach(product => (this.selection.selected.filter((item) => item == product)).length > 0
                                            ? product.include_to_ad = true : product.include_to_ad = false);
    this.taskService.fulfillTask(this.task).subscribe(
      value => console.log(value),
      error => console.error(error),
      () => this.router.navigate(['sales-optimalization'])
    );
  }

}
