import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementPickingComponent } from './advertisement-picking.component';

describe('AdvertisementPickingComponent', () => {
  let component: AdvertisementPickingComponent;
  let fixture: ComponentFixture<AdvertisementPickingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvertisementPickingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertisementPickingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
