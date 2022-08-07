import { Component, OnDestroy, OnInit } from '@angular/core';
import { IMqttMessage, MqttService } from 'ngx-mqtt';
import { Subscription } from 'rxjs';
import { EmployeeServiceService } from 'src/app/services/employee-service.service';
import * as signalR from '@microsoft/signalr';
import { SignalRService } from 'src/app/services/signal-r.service';
import { HttpClient } from '@angular/common/http';

const employees: any[] = [
  {
    photo: 'https://www.dmarge.com/wp-content/uploads/2021/01/dwayne-the-rock-.jpg',
    name: 'Carl Lazlo', position: 'Manager', status: 'Active', time: '13:16:11',
    card_number: 'C1 2F D6 0E', shift: 'Night', access: ['Warehouse', 'Production']
  },

  {
    photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-1-min.png',
    name: 'Steve Michael', position: 'Marketing', status: 'Active', time: '13:15:17',
    card_number: 'FD A9 A1 B3', shift: 'Morning', access: ['Warehouse']
  },

  {
    photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-8-min.png',
    name: 'Gerard Maldonado', position: 'Security', status: 'Active', time: '13:14:37',
    card_number: '9E CD FC 7C', shift: 'Evening', access: ['Entrance A']
  },

  {
    photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-16-min.png',
    name: 'Taylor Hawkins', position: 'HR', status: 'Active', time: '13:12:41',
    card_number: '84 9C 73 AB', shift: 'Morning', access: ['Office']
  },
  {
    photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-20-min.png',
    name: 'Tyrone Skinner', position: 'Security', status: 'Suspended', time: '13:11:29',
    card_number: 'E8 F6 FF 42', shift: 'Morning', access: ['Office #1'],
  },
];

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, OnDestroy {
  employeesArray = employees;
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

  constructor(private _mqttService: MqttService, private empService: EmployeeServiceService,
    public signalRService: SignalRService, private http: HttpClient) {

  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addTransferEmployeeDataListener();
  }

  private startHttpRequest = () => {
    this.http.get('https://controlaccessapp.azurewebsites.net/CheckMonitor')
      .subscribe(res => {
        console.log(res);
      })
  }

  displayNewEntry(): void{
    let myemp: any = {
      photo: 'https://cdn-blbpl.nitrocdn.com/yERRkNKpiDCoDrBCLMpaauJAEtjVyDjw/assets/static/optimized/rev-4899aa8/wp-content/uploads/2021/03/25-Famous-People-Who-Speak-Spanish-as-a-Second-Language-16-min.png',
      name: 'Taylor Hawkins', position: 'HR', status: 'Active', time: '13:12:41',
      card_number: '84 9C 73 AB', shift: 'Morning', access: ['Office']}

    this.employeesArray.pop()
    this.employeesArray.unshift(myemp)
    let device = 'Entrance B'
    if(true){
      switch(device){
        case 'Entrance A': { myemp.style = true; this.cardA_Activation = true; setTimeout(()=>{ this.cardA_Activation = false; myemp.style = false},500)} break;
        case 'Entrance B': { myemp.style = true; this.cardB_Activation = true; setTimeout(()=>{ this.cardB_Activation = false; myemp.style = false},500)} break;
        case 'Warehouse': { myemp.style = true; this.warehouse_Activation = true; setTimeout(()=>{ this.warehouse_Activation = false; myemp.style = false},500)} break;
        case 'Office': { myemp.style = true; this.office_Activation = true; setTimeout(()=>{ this.office_Activation = false; myemp.style = false},500)} break;
      }
    }else{
      switch(device){
        case 'Entrance A': { myemp.styleUn = true; this.cardA_ActivationUn = true; setTimeout(()=>{ this.cardA_ActivationUn = false; myemp.styleUn = false},500)} break;
        case 'Entrance B': { myemp.styleUn = true; this.cardB_ActivationUn = true; setTimeout(()=>{ this.cardB_ActivationUn = false; myemp.styleUn = false},500)} break;
        case 'Warehouse': { myemp.styleUn = true; this.warehouse_ActivationUn = true; setTimeout(()=>{ this.warehouse_ActivationUn = false; myemp.styleUn = false},500)} break;
        case 'Office': { myemp.styleUn = true; this.office_ActivationUn = true; setTimeout(()=>{ this.office_ActivationUn = false; myemp.styleUn = false},500)} break;
      }
    }

    // this.cardA_ActivationUn = true

    console.log(this.employeesArray)

  }

  public unsafePublish(topic: string, message: string): void {
    this._mqttService.unsafePublish(topic, message, {qos: 0, retain: true});
  }

  public ngOnDestroy() {
    //this.subscription.unsubscribe();
    this.signalRService.stopConnection();
  }

}

interface LooseObject {
  [key: string]: any
}

