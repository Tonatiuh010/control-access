import { Component, OnInit } from '@angular/core';
import { MqttService } from 'ngx-mqtt';
import { EmployeeService } from 'src/app/services/employee-service.service';
import { UptimeService } from 'src/app/services/uptime.service';
@Component({
  selector: 'app-entrances-devices',
  templateUrl: './entrances-devices.component.html',
  styleUrls: ['./entrances-devices.component.css']
})
export class EntrancesDevicesComponent implements OnInit {
  apiData!: any[];
  showSpinner!: boolean;
  constructor(private _mqttService: MqttService, private empService: EmployeeService, private uptimeService: UptimeService) {}

  ngOnInit() {
    this.getDevices();
  }

  getDevices(){
    this.showSpinner = true;
    this.empService.getDevices().subscribe(data => {
      this.apiData = data.data;
      this.showSpinner = false;
    });
  }

  getIcon(name: string): string{
    switch(name){
      case 'Entrance A': return '<i style="font-size: 50px;" class="iconSize fa-solid fa-door-closed"></i>';
      case 'Entrance B': return '<i class="iconSize fa-solid fa-door-closed"></i>';
      case 'Warehouse': return '<i class="iconSize fa-solid fa-warehouse"></i>';
      case 'Office': return '<i class="iconSize fa-solid fa-user"></i>';
      case 'Admin': return '<i class="iconSize fa-solid fa-lock"></i>';
      default: return '<i class="iconSize fa-solid fa-door-closed"></i>';
    }
  }

  sendMQTT(status: boolean){
    if(status){
      this.unsafePublish('esp32/toggle', status.toString())
    }else{
      this.unsafePublish('esp32/toggle', status.toString())
    }
  }

  public unsafePublish(topic: string, message: string): void {
    this._mqttService.unsafePublish(topic, message, {qos: 0, retain: true});
  }

  formatDate(date: string): string{
    return this.uptimeService.sendDate(date)
  }
}

