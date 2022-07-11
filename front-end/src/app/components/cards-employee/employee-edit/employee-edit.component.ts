import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Employee } from 'src/app/models/employee-model';
// import {} from './../../../../assets/no-photo-available.png';
@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {

  name = new FormControl('', [Validators.required, Validators.minLength(2)]);
  access = new FormControl('', [Validators.required]);
  position = new FormControl('', [Validators.required]);
  shift = new FormControl('', [Validators.required]);
  card = new FormControl('', [Validators.required]);
  @Input() expectedEmployee!: Employee;
  @Input() employeeInitial!: boolean;

  accessList: string[] = ['HR', 'Office #1', 'Warehouse', 'Production'];
  positionList: string[] = ['Manager', 'Employee', 'Security'];
  cardList: string[] = ['C1 2F D6 0E', 'FD A9 A1 B3', '9E CD FC 7C', '84 9C 73 AB', 'E8 F6 FF 42'];
  shiftList: string[] = ['Morning', 'Evening', 'Night'];

  constructor() { }

  ngOnInit(): void {
  }

  getErrorMessage(field: string): string {
    switch(field){
      case 'name': {
        if (this.name.hasError('required')) {
        return 'You must enter a value';
      }
        return this.name.hasError('minlength') ? 'Name must be longer than 2 characters' : ''
    };

      case 'access': return 'You must select at least 1 option';
      case 'position': return 'You must select an option';
      case 'shift': return 'You must select an option';
      case 'card': return 'You must select an option';
      default : return '';
    }

  }

  onImgError(event: any){
    event.target.src = './../../../../assets/no-photo-available.png'
   }

}
