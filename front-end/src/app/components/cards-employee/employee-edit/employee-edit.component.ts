import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/models/employee-model';
// import {} from './../../../../assets/no-photo-available.png';
@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {


  @Input() expectedEmployee!: Employee;
  @Input() employeeInitial!: boolean;
  form: FormGroup = new FormGroup({});

  accessList: string[] = ['HR', 'Office #1', 'Warehouse', 'Production'];
  positionList: string[] = ['Manager', 'Employee', 'Security'];
  cardList: string[] = ['C1 2F D6 0E', 'FD A9 A1 B3', '9E CD FC 7C', '84 9C 73 AB', 'E8 F6 FF 42'];
  shiftList: string[] = ['Morning', 'Evening', 'Night'];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [null, [Validators.required, Validators.minLength(2)]],
      access: [null, [Validators.required, Validators.minLength(1)]],
      position: [null, [Validators.required]],
      shift: [null, [Validators.required]],
      card: [null, [Validators.required]]
    });
  }

  getErrorMessage(field: string): string {
    switch(field){
      case 'name': {
        if (this.form.get(field)!.hasError('required')) {
        return 'You must enter a value';
      }
        return this.form.get(field)!.hasError('minlength') ? 'Name must be longer than 2 characters' : ''
    };

      case 'access': return 'You must select at least 1 option';
      case 'position': return 'You must select an option';
      case 'shift': return 'You must select an option';
      case 'card': return 'You must select an option';
      default : return '';
    }

  }

  getDepartmentCount():string{
    if(this.form.get('access')!.value === ''){
      return ''
    }
    if(this.form.get('access')!.value?.[0]){
      if((this.form.get('access')!.value?.length || 0) > 1){
        if((this.form.get('access')!.value?.length || 0) > 2){
          return `${this.form.get('access')!.value[0]} (+${this.form.get('access')!.value?.length - 1}  others)`;
        }
        return `${this.form.get('access')!.value[0]} (+${this.form.get('access')!.value?.length - 1}  other)`;
      }
      return this.form.get('access')!.value;
    }
    return this.form.get('access')!.value
  }

  resetForm(): void{
    this.form.reset();
    this.form.markAsPristine
  }

  createEmployee(form: any): void{
    this.form.markAsPristine
    console.log(form)
  }

  updateEmployee(form: any): void{
    this.form.markAsPristine
    console.log(form)
  }

  isValidField(field: string): boolean{
    return ((this.form.get(field)!.touched || this.form.get(field)!.dirty)  && !this.form.get(field)?.valid);
   }

  onImgError(event: any): void{
    event.target.src = './../../../../assets/no-photo-available.png'
   }

}
