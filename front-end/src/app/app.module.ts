import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MapComponent } from './components/map/map.component';
import { CardsEmployeeComponent } from './components/cards-employee/cards-employee.component';
import { NgxEchartsModule } from 'ngx-echarts';
import * as echarts from 'echarts';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './material.module';
import { PrimeNgModule } from './prime-ng.module';
import { EmployeeTableComponent } from './components/cards-employee/employee-table/employee-table.component';
import { AccessCardsTableComponent } from './components/cards-employee/access-cards-table/access-cards-table.component';
import { EmployeeEditComponent } from './components/cards-employee/employee-edit/employee-edit.component';
import { AccessCardsEditComponent } from './components/cards-employee/access-cards-edit/access-cards-edit.component';
import { EntrancesDevicesComponent } from './components/entrances-devices/entrances-devices.component';
import { HistoryComponent } from './components/history/history.component';
import { IMqttServiceOptions, MqttModule } from 'ngx-mqtt';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { httpInterceptProviders } from './components/spinner';

export const MQTT_SERVICE_OPTIONS: IMqttServiceOptions = {
  hostname: 'broker.hivemq.com',
  port: 8000,
  protocol: 'ws',
  path: '/mqtt'
};

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    MapComponent,
    CardsEmployeeComponent,
    EmployeeTableComponent,
    EmployeeEditComponent,
    AccessCardsTableComponent,
    AccessCardsEditComponent,
    EntrancesDevicesComponent,
    HistoryComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MaterialModule,
    PrimeNgModule,
    MqttModule.forRoot(MQTT_SERVICE_OPTIONS),
    NgxEchartsModule.forRoot({
      echarts
    }),
  ],
  providers: [httpInterceptProviders],
  bootstrap: [AppComponent]
})
export class AppModule {

}
