import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';

@Component({
  selector: 'app-access-cards-table',
  templateUrl: './access-cards-table.component.html',
  styleUrls: ['./access-cards-table.component.css']
})
export class AccessCardsTableComponent implements OnInit {
  displayedColumns = ['key', 'employee', 'status',  'options'];
  dataSource:any;

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  apiData!: any;

  constructor(private cdRef: ChangeDetectorRef, private empService: EmployeeServiceService) { }

  ngOnInit(): void {

    this.empService.getCards().subscribe(data =>{
      this.apiData = data;
      this.dataSource = new MatTableDataSource(this.apiData.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.cdRef.detectChanges();
    })

  }
}
