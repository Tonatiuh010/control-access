import { ChangeDetectorRef, Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Output } from '@angular/core';
import { Employee } from 'src/app/models/employee-model';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';

@Component({
  selector: 'app-employee-table',
  templateUrl: './employee-table.component.html',
  styleUrls: ['./employee-table.component.css']
})
export class EmployeeTableComponent implements OnInit {
  displayedColumns = ['name', 'position', 'status', 'card_number', 'options'];
  dataSource: any;
  initial!: boolean;
  emptyEmployee: Employee = {photo: '',
  name: '', position: '', status: '',
  card_number: '', shift:'', access: ['']};

  @Output() employeeSelected = new EventEmitter<Employee>();
  @Output() employeeCreate = new EventEmitter<Employee>();

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  apiData: any;

  constructor(private cdRef: ChangeDetectorRef, private empService: EmployeeServiceService) { }

  ngOnInit(): void {
    this.empService.getEmployees().subscribe(data => {
      this.apiData = data;
    });
    this.dataSource = new MatTableDataSource(data);
    this.cdRef.detectChanges();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    console.log(this.apiData)
  }

  onEmployeeSelect(rowData: Employee) {
    this.employeeSelected.emit({
        photo: rowData.photo,
        name: rowData.name,
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
    this.initial = true;
  }
}

const data: Employee[] = [
  {photo: 'https://www.dmarge.com/wp-content/uploads/2021/01/dwayne-the-rock-.jpg',
  name: 'Carl Lazlo', position: 'Manager', status: 'Active',
  card_number: 'C1 2F D6 0E', shift:'Night', access: ['Warehouse', 'Production']},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-1-min.png',
  name: 'Steve Michael', position: 'Employee', status: 'Active',
  card_number: 'FD A9 A1 B3', shift:'Morning', access: ['Warehouse']},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-8-min.png',
  name: 'Gerard Maldonado', position: 'Security', status: 'Active',
  card_number: '9E CD FC 7C', shift:'Evening', access: ['Production']},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-16-min.png',
  name: 'Taylor Hawkins', position: 'Employee', status: 'Active',
  card_number: '84 9C 73 AB', shift:'Morning', access: ['HR']},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-20-min.png',
  name: 'Tyrone Skinner', position: 'Security', status: 'Suspended',
  card_number: 'E8 F6 FF 42', shift:'Morning', access: ['Office #1']},
];
