import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-access-cards-table',
  templateUrl: './access-cards-table.component.html',
  styleUrls: ['./access-cards-table.component.css']
})
export class AccessCardsTableComponent implements OnInit {
  displayedColumns = ['card_number', 'updated_on', 'status',  'options'];
  dataSource:any;

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private cdRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(data);
    this.cdRef.detectChanges();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}
export interface Card {
  card_number: string;
  status: string;
  updated_on: string;
}

const data: Card[] = [
   {card_number: 'C1 2F D6 0E', status: 'Active', updated_on: '2021-11-08 12:23:54'},
   {card_number: 'FD A9 A1 B3', status: 'Active', updated_on: '2022-07-04 09:36:33'},
   {card_number: '9E CD FC 7C', status: 'Active', updated_on: '2021-12-04 09:45:11'},
   {card_number: '84 9C 73 AB', status: 'Active', updated_on: '2022-06-02 09:11:56'},
   {card_number: 'E8 F6 FF 42', status: 'Disabled', updated_on: '2022-07-12 12:55:34'},
   {card_number: 'C3 B4 F9 21', status: 'Active', updated_on: '2022-07-04 09:36:46'},
   {card_number: 'B2 A4 D3 C4', status: 'Active', updated_on: '2022-07-04 09:36:46'},];
