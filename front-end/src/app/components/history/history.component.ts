import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { History } from 'src/app/models/history-model';
import { EmployeeService } from 'src/app/services/employee-service.service';
import { UptimeService } from 'src/app/services/uptime.service';

@Component({
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  dataSource: any;
  showSpinner!: boolean;
  displayedColumns = ['card.employee.name', 'device.name', 'card.employee.status', 'card.key',
  'card.status', 'checkDt', 'type', 'options'];

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private cdRef: ChangeDetectorRef,
    private empService: EmployeeService,
    private uptimeService: UptimeService) { }

  ngOnInit(): void {
    this.showSpinner = true;
    this.empService.getChecks().subscribe(data => {
      this.dataSource = new MatTableDataSource(data.data);
      this.dataSource.sort = this.sort;
      this.cdRef.detectChanges();
      this.dataSource.paginator = this.paginator;
      this.showSpinner = false;
    });
  }

  sortColumn($event: Sort): void {
    this.dataSource.sortingDataAccessor = (item: any, property: any) => {
      switch (property) {
        case 'card.employee.name': {
          return item.card.employee.name;
        }
        case 'device.name': {
          return item.device.name;
        }
        case 'card.employee.status': {
          return item.card.employee.status;
        }
        case 'card.key': {
          return item.card.key;
        }
        case 'card.status': {
          return item.card.status;
        }
        case 'checkDt': {
          return item.checkDt;
        }
        case 'type': {
          return item.type;
        }
        default: {
          return item[property]; }
      }
    };
}

  getChipColor(status: string):string{
    switch(status){
      case 'ENABLED': return 'green';
      case 'DISABLED': return 'red';
      case 'IN': return 'blue';
      case 'OUT': return 'orange';
      case 'ACCESS': return 'violet';
      default: return 'primary'
    }
  }

  entranceType(type: string, device: string): string{
    switch(type){
      case 'IN': return 'CHECK-IN'
      case 'OUT': return 'CHECK-OUT'
      case 'ACCESS': return this.deviceType(device)
      default: return 'UNKNOWN'
    }
  }

  deviceType(device: string): string{
    switch(device){
      case 'Office': return 'OFFICE'
      case 'Warehouse': return 'WAREHOUSE'
      case 'Entrance A': return 'ACCESS'
      case 'Entrance B': return 'ACCESS'
      default: return 'UNKNOWN'
    }
  }

  formatDate(date: string): string{
    return this.uptimeService.sendDate(date)
  }
}


const data: History[] = [
  {photo: 'https://www.dmarge.com/wp-content/uploads/2021/01/dwayne-the-rock-.jpg',
  name: 'Carl Lazlo', position: 'Manager', employee_status: 'Active',
  card_number: 'C1 2F D6 0E', shift:'Night', access: ['Warehouse', 'Production'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-1-min.png',
  name: 'Steve Michael', position: 'Employee', employee_status: 'Active',
  card_number: 'FD A9 A1 B3', shift:'Morning', access: ['Warehouse'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-Out'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-8-min.png',
  name: 'Gerard Maldonado', position: 'Security', employee_status: 'Active',
  card_number: '9E CD FC 7C', shift:'Evening', access: ['Production'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Lunch'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-16-min.png',
  name: 'Taylor Hawkins', position: 'Employee', employee_status: 'Active',
  card_number: '84 9C 73 AB', shift:'Morning', access: ['HR'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-20-min.png',
  name: 'Tyrone Skinner', position: 'Security', employee_status: 'Suspended',
  card_number: 'E8 F6 FF 42', shift:'Morning', access: ['Office #1'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://www.dmarge.com/wp-content/uploads/2021/01/dwayne-the-rock-.jpg',
  name: 'Carl Lazlo', position: 'Manager', employee_status: 'Active',
  card_number: 'C1 2F D6 0E', shift:'Night', access: ['Warehouse', 'Production'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-1-min.png',
  name: 'Steve Michael', position: 'Employee', employee_status: 'Active',
  card_number: 'FD A9 A1 B3', shift:'Morning', access: ['Warehouse'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-8-min.png',
  name: 'Gerard Maldonado', position: 'Security', employee_status: 'Active',
  card_number: '9E CD FC 7C', shift:'Evening', access: ['Production'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-16-min.png',
  name: 'Taylor Hawkins', position: 'Employee', employee_status: 'Active',
  card_number: '84 9C 73 AB', shift:'Morning', access: ['HR'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},

  {photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-20-min.png',
  name: 'Tyrone Skinner', position: 'Security', employee_status: 'Suspended',
  card_number: 'E8 F6 FF 42', shift:'Morning', access: ['Office #1'], card_status: 'Active', message: 'Entrance A nose', registered_on: '2022-07-24T02:47:57', entrance_type: 'Clock-In'},
];
