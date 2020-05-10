import { Component, OnInit } from '@angular/core';
import {Product} from "../../model/product-model";
import {Observable} from "rxjs";
import {TaskService} from "../../services/task.service";
import {AuthStore} from "../../store/auth.store";

@Component({
  selector: 'app-ordering-cancelation',
  templateUrl: './ordering-cancelation.component.html',
  styleUrls: ['./ordering-cancelation.component.css']
})
export class OrderingCancelationComponent implements OnInit {
  columns: string[] = ['name', 'cancel'];
  //TODO date from to
  actual_season: any = "20.20.2020-25.20.2020";

  dataSource: Product[];

  constructor(private taskService: TaskService, private authStore: AuthStore) {
    this.dataSource = this.taskService.getTask(authStore.getCurrentUser()).products;
  }

  ngOnInit(): void {
  }

}
