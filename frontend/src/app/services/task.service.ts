import {Injectable} from '@angular/core';
import {User} from "../model/user-model";
import {Task, TaskType} from "../model/task-model";
import {Product} from "../model/product-model";
import {Observable, of} from "rxjs";
import {delay} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  mockData: Product[] = [
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},
    {id: '1', product_id: '1', name: 'tester', week_number: 1, price: 10.2,
      sales: 1800, delta_sales: 75, currency: 'eur', new_price: 10.2, include_to_ad: null},

  ];


  constructor() { }

  getTask(user: User): Observable<Task>{
    return of({taskType: TaskType.advertisement_picking, guid: '0', products: this.mockData, scheduledOn: new Date()}).pipe(
      delay(2000)
    );
  }
}
