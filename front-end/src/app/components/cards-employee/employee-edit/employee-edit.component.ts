import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Constants } from 'src/app/models/constants';
import { Employee } from 'src/app/models/employee-model';
import { EmployeeService } from 'src/app/services/employee-service.service';
import { InformationService } from 'src/app/services/information.service';
// import {} from './../../../../assets/no-photo-available.png';
@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit, OnDestroy {

  expectedEmployee!: any;
  employeeInitial!: boolean;
  employeePhoto!: string;
  employeeType!: boolean;

  form: FormGroup = new FormGroup({});
  rstFormEventSubscription!: Subscription;
  subscription1$!: Subscription
  subscription2$!: Subscription
  subscription3$!: Subscription

  accessList: any[] = [];
  jobList: any[] = [];
  cardList: any[] = [];
  shiftList: any[] = [];
  showCard: boolean = false;

  constructor(private fb: FormBuilder, private infService: InformationService,
    private cdRef: ChangeDetectorRef, private empService: EmployeeService) {
    // this.rstFormEventSubscription = this.rstService.getResetForm().subscribe(() =>
    //   this.showCard = true
    // )
  }

  ngOnInit(): void {
    this.subscription1$ = this.infService.selectedEmployee$.subscribe((value) => {
      this.form.markAsPristine();
      this.form.markAsUntouched();
      this.expectedEmployee = value;
      this.employeePhoto = value.photo;
    });

    this.subscription2$ = this.infService.employeeState$.subscribe((value) => {
      this.form.markAsPristine();
      this.form.markAsUntouched();
      this.employeeInitial = value;
    });

    this.subscription3$ = this.infService.employeeType$.subscribe((value) => {
      this.form.markAsPristine();
      this.form.markAsUntouched();
      this.employeeType = value;
    });

      this.empService.getCatalog().subscribe(data => {
        //this.retrieveList(data.data, this.shiftList);
        data.data.forEach((element: { id: any; name: any; }) => {
          this.shiftList.push({id: element.id, name: element.name});
        });

        data.data2.forEach((element: { positionId: any; alias: any; }) => {
          this.jobList.push({id: element.positionId, name: element.alias});
        });

        data.data3.forEach((element: { id: any; key: any; }) => {
          this.cardList.push({id: element.id, key: element.key});
        });

        data.data4.forEach((element: { id: any; name: any; }) => {
          this.accessList.push({id: element.id, name: element.name});
        });

      });

    this.form = this.fb.group({
      id: [null],
      name: [null, [Validators.required, Validators.minLength(2), Validators.pattern(/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$/u)]],
      image: [null],
      lastName: [null, [Validators.required, Validators.minLength(2), Validators.pattern(/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$/u)]],
      accessLevels: [null, [Validators.required, Validators.minLength(1)]],
      position: [null, [Validators.required]],
      shift: [null, [Validators.required]],
      card: [null]
    });
  }

  ngOnDestroy() {
    this.subscription1$.unsubscribe()
    this.subscription2$.unsubscribe()
    this.subscription3$.unsubscribe()
  }

  getErrorMessage(field: string): string {
    switch(field){
      case 'name': case 'lastName':{
        if (this.form.get(field)!.hasError('required')) {
        return 'You must enter a value';
      }
        if (this.form.get(field)!.hasError('pattern')) {
        return 'Field must only contain letters';
       }

        return this.form.get(field)!.hasError('minlength') ? 'Name must be longer than 2 characters' : ''
    };

      case 'accessLevels': return 'You must select at least 1 option';
      case 'position': return 'You must select an option';
      case 'shift': return 'You must select an option';
      case 'card': return 'You must select an option';
      default : return '';
    }

  }

  getDepartmentCount():string{
    if(this.form.get('accessLevels')!.value === ''){
      return ''
    }
    if(this.form.get('accessLevels')!.value?.[0]){
      if((this.form.get('accessLevels')!.value?.length || 0) > 1){
        if((this.form.get('accessLevels')!.value?.length || 0) > 2){
          return `${this.form.get('accessLevels')!.value[0]} (+${this.form.get('accessLevels')!.value?.length - 1}  others)`;
        }
        return `${this.form.get('accessLevels')!.value[0]} (+${this.form.get('accessLevels')!.value?.length - 1}  other)`;
      }
      return this.form.get('accessLevels')!.value;
    }
    return this.form.get('accessLevels')!.value
  }

  resetForm(): void{
    this.infService.sendEmployeeState(false);
  }

  createEmployee(form: any): void{
    form.value.id = null;
    this.form.value.card = this.form.value.card.id;
    if (this.employeePhoto){
      form.value.image = this.employeePhoto
    } else{
      form.value.image = Constants['no-image-found']
    }

    this.empService.postEmployee(form.value).subscribe(data => {
      this.resetForm();
    })
  }

  updateEmployee(form: any): void{
    this.form.markAsPristine

    if (form.value.card){
      if (this.isInt(form.value.card)){
        form.value.card = form.value.card
      }else{
        form.value.card = form.value.card.id
      }
    }

    if (this.employeePhoto){
      form.value.image = this.employeePhoto
    } else{
      form.value.image = Constants['no-image-found']
    }
    this.empService.updateEmployee(form.value).subscribe(data => {
      this.resetForm();
    })
  }

  isInt(value: any) {
    var x;
    if (isNaN(value)) {
      return false;
    }
    x = parseFloat(value);
    return (x | 0) === x;
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

}
