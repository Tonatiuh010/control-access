import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Constants } from 'src/app/models/constants';
import { Employee } from 'src/app/models/employee-model';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';
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

  accessList: string[] = [];
  positionList: string[] = [];
  cardList: string[] = [];
  shiftList: string[] = [];
  showCard: boolean = false;

  constructor(private fb: FormBuilder, private rstService: ResetFormService,
    private cdRef: ChangeDetectorRef, private empService: EmployeeServiceService) {
    this.rstFormEventSubscription = this.rstService.getResetForm().subscribe(() =>
      this.showCard = true
    )
  }

  ngOnInit(): void {
    setTimeout(()=>{
      this.empService.getCatalog().subscribe(data => {
        this.retrieveList(data.data, this.shiftList);
        this.retrieveList(data.data2, this.positionList);
        data.data3.forEach((element: { key: string; }) => {
          this.cardList.push(element.key);
        });
        this.retrieveList(data.data4, this.accessList);
      });
    }, 1000)

    this.form = this.fb.group({
      id: [null],
      name: [null, [Validators.required, Validators.minLength(2)]],
      upload: [null],
      lastName: [null, [Validators.required, Validators.minLength(2)]],
      access: [null, [Validators.required, Validators.minLength(1)]],
      position: [null, [Validators.required]],
      shift: [null, [Validators.required]],
      card: [null, [Validators.required]]
    });
  }

  retrieveList(data: any[], list: string[]):void {
    data.forEach((element: { name: string; }) => {
      list.push(element.name);
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
    this.showCard = false;
  }

  createEmployee(form: any): void{
    form.value.id = null;
    if (this.employeePhoto){
      form.value.upload = this.employeePhoto
    } else{
      form.value.upload = Constants['no-image-found']
    }
    console.log(form.value)
    this.expectedEmployee = undefined;
    this.resetForm();
  }

  updateEmployee(form: any): void{
    this.form.markAsPristine
    if (this.employeePhoto){
      form.value.upload = this.employeePhoto
    } else{
      form.value.upload = Constants['no-image-found']
    }
    console.log(form.value)
  }

  isValidField(field: string): boolean{
    return ((this.form.get(field)!.touched || this.form.get(field)!.dirty)  && !this.form.get(field)?.valid);
   }

  onImgError(event: any): void{
    event.target.src = './../../../../assets/no-photo-available.png'
   }

   imageUpload(event:any){
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

  photoState(){
    console.log(this.form.get('shift')!.value)
  }

}
