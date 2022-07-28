import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Output } from '@angular/core';
import { Employee } from 'src/app/models/employee-model';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';
import { ResetFormService } from 'src/app/services/reset-form.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-employee-table',
  templateUrl: './employee-table.component.html',
  styleUrls: ['./employee-table.component.css']
})
export class EmployeeTableComponent implements OnInit {
  displayedColumns = ['name', 'position', 'status', 'card_number', 'options'];
  dataSource =  new MatTableDataSource<any>([]);;
  initial!: boolean;
  emptyEmployee: Employee = {photo: '',
  name: '', position: '', status: '',
  card_number: '', shift:'', access: ['']};
  apiData!: any;

  @Output() employeeSelected = new EventEmitter<any>();
  @Output() employeeCreate = new EventEmitter<any>();

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private cdRef: ChangeDetectorRef, private empService: EmployeeServiceService, private rstService: ResetFormService){ };

  ngOnInit(): void {
    this.empService.getEmployees().subscribe(data => {
      this.apiData = data;
      this.dataSource = new MatTableDataSource(this.apiData.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.cdRef.detectChanges();
    });
  }

  onEmployeeSelect(rowData: any) {
    this.employeeSelected.emit({
        photo: rowData.photo,
        name: rowData.name,
        lastName: rowData.lastName,
        position: rowData.position,
        status: rowData.status,
        card_number: rowData.card_number,
        shift: rowData.shift,
        access: rowData.access
      });
      this.initial = false;
  }

  onEmployeeCreate() {
    this.employeeCreate.emit(this.emptyEmployee);
    this.rstService.sendResetForm();
    this.initial = true;
  }

  onImgError(event: any): void{
    event.target.src = './../../../../assets/no-photo-available.png'
   }
}

