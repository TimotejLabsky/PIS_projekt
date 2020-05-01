import { Injectable } from '@angular/core';
import {Product} from "../model/product-model";
import {Observable, of, interval} from "rxjs";
import {debounce} from "rxjs/operators";



@Injectable({
  providedIn: 'root'
})
export class ProductService {
  mockData: Product[] = [
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
    {name: 'te', old_price: 5, sales: 500, delta_sales: 25, new_price:5},
    {name: 'te1', old_price: 51, sales: 10, delta_sales: 25, new_price:5},
    {name: 'te2', old_price: 25, sales: 800, delta_sales: 25, new_price:5},
  ];

  constructor() { }

  getAllProducts(): Observable<Product[]>{
    //TODO backend call
    return of(this.mockData).pipe(debounce(() => interval(100000)));
  }
}
