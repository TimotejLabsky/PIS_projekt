import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncludeToSeasonComponent } from './include-to-season.component';

describe('IncludeToSeasonComponent', () => {
  let component: IncludeToSeasonComponent;
  let fixture: ComponentFixture<IncludeToSeasonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncludeToSeasonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncludeToSeasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
