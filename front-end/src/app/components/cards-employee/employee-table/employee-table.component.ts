import { ChangeDetectorRef, Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';
import { InformationService } from 'src/app/services/information.service';

@Component({
  selector: 'app-employee-table',
  templateUrl: './employee-table.component.html',
  styleUrls: ['./employee-table.component.css']
})
export class EmployeeTableComponent implements OnInit {
  displayedColumns = ['name', 'position', 'status', 'card_number', 'options'];
  dataSource: any;
  emptyEmployee = {photo: undefined,
  name: '', position: '', status: '',
  card_number: '', shift: '', access: ['']};
  apiData!: any;

  showSpinner = false;

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private cdRef: ChangeDetectorRef, private empService: EmployeeServiceService,
    private infService: InformationService){ };

  ngOnInit(): void {

    this.getEmployees();
  }

  getEmployees(){
    this.showSpinner = true;
    this.empService.getEmployees().subscribe(data => {
      this.apiData = data;
      this.dataSource = new MatTableDataSource(this.apiData.data);
      this.dataSource.sort = this.sort;
      this.cdRef.detectChanges();
      this.dataSource.paginator = this.paginator;
      this.showSpinner = false;
    })
  }

  onEmployeeSelect(rowData: any) {
    const arr: string[] = [];
    rowData.accessLevels.forEach((element: { name: string; }) => {
      arr.push(element.name)
    });

    this.infService.sendEmployee({
      id: rowData.id,
      photo: rowData.image.url,
      name: rowData.name,
      lastName: rowData.lastName,
      position: rowData.job.id,
      status: rowData.status,
      card_number: rowData.card,
      shift: rowData.shift.id,
      accessLevels: arr
    });
    this.infService.sendEmployeeState(true);
    this.infService.sendType(true);
  }

  onEmployeeCreate() {
    this.infService.sendEmployee(this.emptyEmployee);
    this.infService.sendEmployeeState(true);
    this.infService.sendType(false);
  }

  disableEmployee(rowData: any) {
    let message = {id: rowData}
    this.empService.disableEmployee(message).subscribe(data => {
      console.log(data)
    })
  }

  onImgError(event: any): void{
    event.target.src = './../../../../assets/no-photo-available.png'
   }


}

