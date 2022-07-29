import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Constants } from 'src/app/models/constants';
import { Employee } from 'src/app/models/employee-model';
import { ResetFormService } from 'src/app/services/reset-form.service';
// import {} from './../../../../assets/no-photo-available.png';
@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {


  @Input() expectedEmployee!: any;
  @Input() employeeInitial!: boolean;
  @Input() employeePhoto!: any;
  form: FormGroup = new FormGroup({});
  rstFormEventSubscription!: Subscription;

  accessList: any[] = ['G1', 'G2', 'G3', 'Production'];
  positionList: string[] = ['Manager', 'Employee', 'Security'];
  cardList: string[] = ['C1 2F D6 0E', 'FD A9 A1 B3', '9E CD FC 7C', '84 9C 73 AB', 'E8 F6 FF 42'];
  shiftList: string[] = ['MATUTINE', 'Evening', 'Night'];
  //employeePhoto: any = this.expectedEmployee;

  constructor(private fb: FormBuilder, private rstService: ResetFormService, private cdRef: ChangeDetectorRef) {
    this.rstFormEventSubscription = this.rstService.getResetForm().subscribe(() =>
      this.resetForm()
    )
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [null, [Validators.required, Validators.minLength(2)]],
      upload: [null],
      lastName: [null, [Validators.required, Validators.minLength(2)]],
      access: [null, [Validators.required, Validators.minLength(1)]],
      position: [null, [Validators.required]],
      shift: [null, [Validators.required]],
      card: [null, [Validators.required]]
    });
  }

  getErrorMessage(field: string): string {
    switch(field){
      case 'name': case 'lastName':{
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
    console.log(this.employeePhoto)
    this.employeePhoto = undefined
  }

  updateEmployee(form: any): void{
    this.form.markAsPristine
    if (this.employeePhoto){
      console.log('hola')
      form.value.upload = this.employeePhoto
    } else{
      form.value.upload = Constants['no-image-found']
    }
    console.log(form)
    this.employeePhoto = undefined
  }

  isValidField(field: string): boolean{
    return ((this.form.get(field)!.touched || this.form.get(field)!.dirty)  && !this.form.get(field)?.valid);
   }

  onImgError(event: any): void{
    event.target.src = './../../../../assets/no-photo-available.png'
   }

   imageUpload(event:any)
  {
    var file = event.target.files.length;
    for(let i=0;i<file;i++)
    {
       var reader = new FileReader();
       reader.onload = (event:any) =>
       {
           this.employeePhoto = event.target.result;
           this.cdRef.detectChanges();
       }
       reader.readAsDataURL(event.target.files[i]);
    }
  }

}
