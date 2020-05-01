import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import {ProductStore} from "../../store/product.store";

@Component({
  selector: 'app-advertisement-picking',
  templateUrl: './advertisement-picking.component.html',
  styleUrls: ['./advertisement-picking.component.css']
})
export class AdvertisementPickingComponent implements OnInit {
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];
  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  dataSource: Observable<Product[]>

  constructor(private productStore: ProductStore) {
    this.dataSource = productStore.products$;
  }

  ngOnInit(): void {
  }

}
