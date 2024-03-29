import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection!: signalR.HubConnection
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

    public addTransferChartDataListener = () => {
      this.hubConnection.on('test', (data) => {
        this.data = data;
        console.log(data);
      });
    }
}
