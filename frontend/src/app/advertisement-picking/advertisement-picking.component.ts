import { Component, OnInit } from '@angular/core';
import {ProductPriceUpdate} from "../model/product_price_update-model";

@Component({
  selector: 'app-advertisement-picking',
  templateUrl: './advertisement-picking.component.html',
  styleUrls: ['./advertisement-picking.component.css']
})
export class AdvertisementPickingComponent implements OnInit {
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];

  dataSource: ProductPriceUpdate[] = [
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
