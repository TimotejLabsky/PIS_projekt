import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import {ProductService} from "../../services/product.service";

@Component({
  selector: 'app-ordering-cancelation',
  templateUrl: './ordering-cancelation.component.html',
  styleUrls: ['./ordering-cancelation.component.css']
})
export class OrderingCancelationComponent implements OnInit {
  columns: string[] = ['name', 'cancel'];
  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  dataSource: Observable<Product[]>;

  constructor(private productService: ProductService) {
    this.dataSource = this.productService.getAllProducts();
  }

  ngOnInit(): void {
  }

}
