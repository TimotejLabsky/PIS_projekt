import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-include-to-season',
  templateUrl: './include-to-season.component.html',
  styleUrls: ['./include-to-season.component.css']
})
export class IncludeToSeasonComponent implements OnInit {
  actual_season: any;
  dataSource: any;
  columns: string[] = ['name', 'old_price', 'sales', 'delta_sales', 'include'];

  constructor() { }

  ngOnInit(): void {
  }

}
