import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { InformationService } from 'src/app/services/information.service';
import { IMqttMessage, MqttService } from 'ngx-mqtt';

@Component({
  selector: 'app-access-cards-edit',
  templateUrl: './access-cards-edit.component.html',
  styleUrls: ['./access-cards-edit.component.css']
})
export class AccessCardsEditComponent implements OnInit {

  subscription1$!: Subscription
  cardActivation: boolean = false;

  constructor(private infService: InformationService, private _mqttService: MqttService) { }

  ngOnInit(): void {
    this.subscription1$ = this.infService.cardActivion$.subscribe((value) => {
      this.cardActivation = value;
    });
  }

  resetCard(){
    this.infService.sendActivation(false)
    this.unsafePublish('esp32/admin', 'false')
  }

  public unsafePublish(topic: string, message: string): void {
    this._mqttService.unsafePublish(topic, message, {qos: 0, retain: true});
  }

}
