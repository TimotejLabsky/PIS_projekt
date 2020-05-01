import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";

@Component({
  selector: 'app-ordering-cancelation',
  templateUrl: './ordering-cancelation.component.html',
  styleUrls: ['./ordering-cancelation.component.css']
})
export class OrderingCancelationComponent implements OnInit {
  columns: string[] = ['name', 'cancel'];
  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

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


  constructor() { }

  ngOnInit(): void {
  }

}
