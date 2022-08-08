import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { MapComponent } from '../components/map/map.component';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  public hubConnection!: signalR.HubConnection
  data!: string;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://controlaccessapp.azurewebsites.net/CheckMonitor')
                              .build();
    this.hubConnection
      .start()
      .then(() => this.hubConnection.invoke("BroadcastGreeting", "sam"))
      .catch(err => console.log('Error while starting connection: ' + err))
    }

    public addTransferEmployeeDataListener(attr1 = (_: any, map: MapComponent) => {}, map: MapComponent){
      this.hubConnection.on('CheckMonitor', (data) => {
        this.data = data;
        attr1(this.data, map);
      });
    }

    public stopConnection = () => {
      this.hubConnection.off('CheckMonitor')
    }
}
