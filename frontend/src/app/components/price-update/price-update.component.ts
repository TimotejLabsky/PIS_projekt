import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";
import {ProductStore} from "../../store/product.store";
import {Observable} from "rxjs";

@Component({
  selector: 'app-price-update',
  templateUrl: './price-update.component.html',
  styleUrls: ['./price-update.component.css']
})
export class PriceUpdateComponent implements OnInit {
  columns: string[]  = ['name', 'old_price', 'sales', 'delta_sales', 'new_price'];

  dataSource: Observable<Product[]>;

  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  constructor(private productStore: ProductStore) {
    this.dataSource = this.productStore.products$;
  }

  ngOnInit(): void {
  }

}
