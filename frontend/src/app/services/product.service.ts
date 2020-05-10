import { Injectable } from '@angular/core';
import {Product} from "../model/product-model";
import {Observable, of, interval} from "rxjs";
import {debounce} from "rxjs/operators";



@Injectable({
  providedIn: 'root'
})
export class ProductService {
  mockData: Product[] = [
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2}
  ];

  constructor() { }

  getAllProducts(): Observable<Product[]>{
    //TODO backend call
    return of(this.mockData).pipe(debounce(() => interval(100000)));
  }
}
