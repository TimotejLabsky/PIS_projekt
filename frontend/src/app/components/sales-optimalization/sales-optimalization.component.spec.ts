import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesOptimalizationComponent } from './sales-optimalization.component';

describe('HomeComponent', () => {
  let component: SalesOptimalizationComponent;
  let fixture: ComponentFixture<SalesOptimalizationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesOptimalizationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesOptimalizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
