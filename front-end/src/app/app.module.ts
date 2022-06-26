import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MapComponent } from './components/map/map.component';
import { CardsEmployeeComponent } from './components/cards-employee/cards-employee.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    MapComponent,
    CardsEmployeeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
