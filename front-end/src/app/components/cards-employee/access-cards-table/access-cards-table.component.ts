import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeService } from 'src/app/services/employee-service.service';
import { InformationService } from 'src/app/services/information.service';
import { IMqttMessage, MqttService } from 'ngx-mqtt';

@Component({
  selector: 'app-access-cards-table',
  templateUrl: './access-cards-table.component.html',
  styleUrls: ['./access-cards-table.component.css']
})
export class AccessCardsTableComponent implements OnInit {
  displayedColumns = ['key', 'employee', 'status',  'options'];
  dataSource: any;

  @ViewChild('examplePaginator') paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  apiData!: any;
  showSpinner: boolean  = false;

  constructor(private cdRef: ChangeDetectorRef, private empService: EmployeeService,
    private infService: InformationService, private _mqttService: MqttService) { }

  ngOnInit(): void {
    this.getCards();
  }

  getCards(){
    this.showSpinner = true;
    this.empService.getCards().subscribe(data =>{
      this.apiData = data;
      this.dataSource = new MatTableDataSource(this.apiData.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.cdRef.detectChanges();
      this.showSpinner = false;
    })
  }

  addCard(){
    this.infService.sendActivation(true)
    this.unsafePublish('esp32/admin', 'true')
  }

  public unsafePublish(topic: string, message: string): void {
    this._mqttService.unsafePublish(topic, message, {qos: 0, retain: true});
  }
}
