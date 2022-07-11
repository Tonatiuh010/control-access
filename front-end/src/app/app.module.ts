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

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    MapComponent,
    CardsEmployeeComponent,
    EmployeeTableComponent,
    EmployeeEditComponent,
    AccessCardsTableComponent,
    AccessCardsEditComponent
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
    NgxEchartsModule.forRoot({
      echarts
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {

}
