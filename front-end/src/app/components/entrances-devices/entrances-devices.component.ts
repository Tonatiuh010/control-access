import { Component, OnInit } from '@angular/core';
import { MqttService } from 'ngx-mqtt';

@Component({
  selector: 'app-entrances-devices',
  templateUrl: './entrances-devices.component.html',
  styleUrls: ['./entrances-devices.component.css']
})
export class EntrancesDevicesComponent implements OnInit {
  propiedadChafa: any;
  constructor(private _mqttService: MqttService) {}

  ngOnInit() {
    this.propiedadChafa = MOCK
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
}

const MOCK = [{
  name: 'Entrance A',
  status: true,
  last_update: '2020-03-08 00:00:00',
  activations: 103,
  unsuccessful: 7
},{
  name: 'Entrance B',
  status: true,
  last_update: '2020-03-08 00:00:00',
  activations: 364,
  unsuccessful: 11
},{
  name: 'Warehouse',
  status: true,
  last_update: '2020-03-08 00:00:00',
  activations: 52,
  unsuccessful: 1
},{
  name: 'Office',
  status: true,
  last_update: '2020-03-08 00:00:00',
  activations: 14,
  unsuccessful: 0
},{
  name: 'Admin',
  status: true,
  last_update: '2020-03-08 00:00:00',
  activations: 3,
  unsuccessful: 0
},]
