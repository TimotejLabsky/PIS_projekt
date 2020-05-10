import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";

@Component({
  selector: 'app-price-update',
  templateUrl: './price-update.component.html',
  styleUrls: ['./price-update.component.css']
})
export class PriceUpdateComponent implements OnInit {
  columns: string[]  = ['name', 'old_price', 'sales', 'delta_sales', 'new_price'];

  dataSource: Product[];

  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    this.dataSource = this.taskService.getTask(authStore.getCurrentUser()).products;
  }
  ngOnInit(): void {
  }

}
