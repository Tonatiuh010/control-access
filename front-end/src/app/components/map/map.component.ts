import { ChangeDetectorRef, Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { IMqttMessage, MqttService } from 'ngx-mqtt';
import { Subscription } from 'rxjs';
import { EmployeeService } from 'src/app/services/employee-service.service';
import * as signalR from '@microsoft/signalr';
import { SignalRService } from 'src/app/services/signal-r.service';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar, MatSnackBarConfig, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { UptimeService } from 'src/app/services/uptime.service';


@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, OnDestroy {
  employeesArray: any[] = [];;
  private subscription!: Subscription;
  public message!: string;
  apiData!: any[];
  newArray!: any[];
  cardA_Activation: boolean = false;
  cardA_ActivationUn: boolean = false;
  cardB_Activation: boolean = false;
  cardB_ActivationUn: boolean = false;
  warehouse_Activation: boolean = false;
  warehouse_ActivationUn: boolean = false;
  office_Activation: boolean = false;
  office_ActivationUn: boolean = false;
  hubConnection = this.signalRService.hubConnection;
  rawData: Check[] = [];
  status: string[] = [];
  horizontalPosition: MatSnackBarHorizontalPosition = 'center';
  verticalPosition: MatSnackBarVerticalPosition = 'bottom';
  constructor(private _mqttService: MqttService,
    public signalRService: SignalRService, private cdRef: ChangeDetectorRef,
    private _snackBar: MatSnackBar, private zone: NgZone, private uptimeService: UptimeService) {

  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addTransferEmployeeDataListener(this.myCallback, this)
  }

   myCallback(res: CheckStats, map: MapComponent) {
    map.displayNewEntry(res.status, res.isValid, res.check)
  }

  displayNewEntry(status: string, isValid: boolean, data: Check): void{
    let device = data.device.name;
    if (!data.card) {

      const config = new MatSnackBarConfig();
      config.horizontalPosition = 'center';
      config.verticalPosition = 'bottom';
      config.duration = 4000;
      this.zone.run(() => {
        this._snackBar.open(`Unknown/Unassigned card detected on device ${device}.`, 'Dismiss', config)
        switch(device){
          case 'Entrance A': { this.cardA_ActivationUn = true; setTimeout(()=>{ this.cardA_ActivationUn = false; this.cdRef.detectChanges();},500)} break;
          case 'Entrance B': {  this.cardB_ActivationUn = true; setTimeout(()=>{ this.cardB_ActivationUn = false; myemp.styleUn = false; this.cdRef.detectChanges();},500)} break;
          case 'Warehouse': { this.warehouse_ActivationUn = true; setTimeout(()=>{ this.warehouse_ActivationUn = false; this.cdRef.detectChanges();},500)} break;
          case 'Office': {  this.office_ActivationUn = true; setTimeout(()=>{ this.office_ActivationUn = false; this.cdRef.detectChanges();},500)} break;
        }
      });
      return;
    }
    let myemp: any = data.card.employee
    this.employeesArray.unshift(myemp)
    if(isValid){
      this.status.unshift(status);
      this.rawData.unshift(data);
      switch(device){
        case 'Entrance A': { myemp.style = true; this.cardA_Activation = true; setTimeout(()=>{ this.cardA_Activation = false; myemp.style = false; this.cdRef.detectChanges();},500); } break;
        case 'Entrance B': { myemp.style = true; this.cardB_Activation = true; setTimeout(()=>{ this.cardB_Activation = false; myemp.style = false; this.cdRef.detectChanges();},500)} break;
        case 'Warehouse': { myemp.style = true; this.warehouse_Activation = true; setTimeout(()=>{ this.warehouse_Activation = false; myemp.style = false; this.cdRef.detectChanges();},500)} break;
        case 'Office': { myemp.style = true; this.office_Activation = true; setTimeout(()=>{ this.office_Activation = false; myemp.style = false; this.cdRef.detectChanges();},500)} break;
      }
    }else{
      this.status.unshift(status);
      this.rawData.unshift(data);
      switch(device){
        case 'Entrance A': { myemp.styleUn = true; this.cardA_ActivationUn = true; setTimeout(()=>{ this.cardA_ActivationUn = false; myemp.styleUn = false; this.cdRef.detectChanges();},500)} break;
        case 'Entrance B': { myemp.styleUn = true; this.cardB_ActivationUn = true; setTimeout(()=>{ this.cardB_ActivationUn = false; myemp.styleUn = false; this.cdRef.detectChanges();},500)} break;
        case 'Warehouse': { myemp.styleUn = true; this.warehouse_ActivationUn = true; setTimeout(()=>{ this.warehouse_ActivationUn = false; myemp.styleUn = false; this.cdRef.detectChanges();},500)} break;
        case 'Office': { myemp.styleUn = true; this.office_ActivationUn = true; setTimeout(()=>{ this.office_ActivationUn = false; myemp.styleUn = false; this.cdRef.detectChanges();},500)} break;
      }
    }

    this.cdRef.detectChanges();
  }

  public unsafePublish(topic: string, message: string): void {
    this._mqttService.unsafePublish(topic, message, {qos: 0, retain: true});
  }

  public ngOnDestroy() {
    //this.subscription.unsubscribe();
    this.signalRService.stopConnection();
  }

  formatDate(date: string): string{
    return this.uptimeService.sendDate(date)
  }
}

interface LooseObject {
  [key: string]: any
}

export interface CheckStats{
  check: Check;
  isValid: boolean;
  message: string;
  status: string;
}

export interface Check {
  device?: any;
  checkDt: Date;
  type: string;
  card?: any;
  id?: any;
}

