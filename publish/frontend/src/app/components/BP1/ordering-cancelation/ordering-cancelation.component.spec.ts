import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderingCancelationComponent } from './ordering-cancelation.component';

describe('OrderingCancelationComponent', () => {
  let component: OrderingCancelationComponent;
  let fixture: ComponentFixture<OrderingCancelationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderingCancelationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderingCancelationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
