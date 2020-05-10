import {Injectable} from '@angular/core';
import {User} from "../model/user-model";
import {Task, TaskType} from "../model/task-model";
import {Product} from "../model/product-model";
import {Observable, of} from "rxjs";
import {delay} from "rxjs/operators";
import {base_endpoint} from "../url.constatns";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private endpoint: string = base_endpoint + '/tasks';


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


  constructor(private httpClient: HttpClient) { }

  getTask(user: User): Observable<Task>{
    return this.httpClient.get<Task>(this.endpoint + '/next')

    return of({taskType: TaskType.advertisement_picking, guid: '0', products: this.mockData, scheduledOn: new Date()}).pipe(
      delay(2000)
    );
  }

  fulfillTask(task: Task){
    return this.httpClient.post(this.endpoint + '/fulfill', task)
  }

}
