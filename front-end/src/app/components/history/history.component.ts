import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { History } from 'src/app/models/history-model';

@Component({
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  dataSource: any;
  displayedColumns = ['name', 'position', 'employee_status', 'card_number',
  'card_status', 'message', 'registered_on', 'entrance_type', 'options'];

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private cdRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(data);
    this.cdRef.detectChanges();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  getChipColor(status: string):string{
    switch(status){
      case 'Active': return 'green';
      case 'Suspended': return 'red';
      case 'Disabled': return 'red';
      case 'Clock-In': return 'blue';
      case 'Clock-Out': return 'orange';
      case 'Lunch': return 'violet';
      default: return 'primary'
    }
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
