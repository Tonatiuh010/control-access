import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Employee } from 'src/app/models/employee-model';

@Component({
  selector: 'app-cards-employee',
  templateUrl: './cards-employee.component.html',
  styleUrls: ['./cards-employee.component.css']
})
export class CardsEmployeeComponent implements OnInit {

  employee!: Employee;
  @Output() employeeInitial = new EventEmitter<boolean>();
  initial!: boolean;
  constructor() { }

  ngOnInit(): void {
  }

  onEmployeeSelect(eventData: Employee) {
    this.employee = eventData;
    this.initial = false
  }

  onEmployeeCreate(eventData: Employee) {
    this.employee = eventData;
    this.initial = true
  }

}
