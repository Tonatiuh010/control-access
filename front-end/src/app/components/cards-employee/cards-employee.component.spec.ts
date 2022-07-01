import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsEmployeeComponent } from './cards-employee.component';

describe('CardsEmployeeComponent', () => {
  let component: CardsEmployeeComponent;
  let fixture: ComponentFixture<CardsEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardsEmployeeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardsEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
