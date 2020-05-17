import { Component, OnInit } from '@angular/core';
import {Product} from "../../../model/product-model";
import {Observable} from "rxjs";
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";
import { Task } from 'src/app/model/task-model';
import {Router} from "@angular/router";
import {TaskStore} from "../../../store/task.store";

@Component({
  selector: 'app-price-update',
  templateUrl: './price-update.component.html',
  styleUrls: ['./price-update.component.css']
})
export class PriceUpdateComponent implements OnInit {
  columns: string[]  = ['name', 'old_price', 'sales', 'delta_sales', 'new_price'];

  dataSource: Product[];
  loading: boolean = false;
  private task: Task;

  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  constructor(private taskService: TaskService, private authStore: AuthStore,
              private router: Router, private taskStore: TaskStore) {

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

    this.taskService.fulfillTask(this.task).subscribe(
      value => console.log(value),
      error => console.error(error),
      () => this.router.navigate(['sales-optimalization'])
    );
  }

  priceChange($event: any, element: any) {
    element.new_price = Number($event);
  }
}
