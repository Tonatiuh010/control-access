import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Employee } from 'src/app/models/employee-model';
import { ResetFormService } from 'src/app/services/reset-form.service';

@Component({
  selector: 'app-cards-employee',
  templateUrl: './cards-employee.component.html',
  styleUrls: ['./cards-employee.component.css']
})
export class CardsEmployeeComponent implements OnInit {

  employee!: any;
  initial!: boolean;
  photo!: any;
  constructor(private rstService: ResetFormService) { }

  ngOnInit(): void {
  }

  employeeSelector(eventData: any){
    if (eventData.name){
      this.onEmployeeSelect(eventData);
    }else{
      this.onEmployeeCreate(eventData);
    }
  }

  onEmployeeSelect(eventData: any) {
    this.employee = eventData;
    this.initial = false
    this.rstService.sendResetForm()
    this.photo = eventData.photo
    console.log('select')
  }

  onEmployeeCreate(eventData: any) {
    this.employee = eventData;
    this.initial = true
    this.rstService.sendResetForm()
    this.photo = undefined
    console.log('create')
  }

}
