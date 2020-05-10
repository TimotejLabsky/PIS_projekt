import {Component, OnInit} from '@angular/core';
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import {map} from 'rxjs/operators';
import {ProductService} from "../../services/product.service";
import { MatTableDataSource } from "@angular/material/table";

@Component({
  selector: 'app-advertisement-picking',
  templateUrl: './advertisement-picking.component.html',
  styleUrls: ['./advertisement-picking.component.css']
})
export class AdvertisementPickingComponent implements OnInit {
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include']; //TODO
  dataSource: MatTableDataSource<Product>
  actual_season: string = "20.20.2020-25.20.2020";

  constructor(private productService: ProductService) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(products => {
      this.dataSource.data = products;
    })
  }

  applyFilter($event: KeyboardEvent) {
    const filterValue = ($event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
