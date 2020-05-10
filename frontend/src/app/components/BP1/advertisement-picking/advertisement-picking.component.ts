import {Component, Input, OnInit} from '@angular/core';
import {Product} from "../../../model/product-model";
import {Observable} from "rxjs";
import {map} from 'rxjs/operators';
import { MatTableDataSource } from "@angular/material/table";
import {TaskService} from "../../../services/task.service";
import {AuthStore} from "../../../store/auth.store";

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

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    let products: Product[];
    this.loading = true;

    this.taskService.getTask(authStore.getCurrentUser()).subscribe(
      tasks => products = tasks.products,
      err => console.error(err),
      () => this.loading = false
    )

    this.dataSource = new MatTableDataSource(products);
  }

  ngOnInit(): void {

  }

  applyFilter($event: KeyboardEvent) {
    const filterValue = ($event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
