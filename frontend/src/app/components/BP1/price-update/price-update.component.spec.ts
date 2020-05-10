import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {MatTableModule} from "@angular/material/table";

import { PriceUpdateComponent } from './price-update.component';

describe('PriceUpdateComponent', () => {
  let component: PriceUpdateComponent;
  let fixture: ComponentFixture<PriceUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PriceUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PriceUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
