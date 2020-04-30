import { Component, OnInit } from '@angular/core';
import {Product} from "../model/product-model";

@Component({
  selector: 'app-price-update',
  templateUrl: './price-update.component.html',
  styleUrls: ['./price-update.component.css']
})
export class PriceUpdateComponent implements OnInit {
  columns: string[]  = ['name', 'old_price', 'sales', 'delta_sales', 'new_price'];

  dataSource: Product[] = [
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

  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  constructor() {
    console.log(this.dataSource);
  }

  ngOnInit(): void {
  }

}
