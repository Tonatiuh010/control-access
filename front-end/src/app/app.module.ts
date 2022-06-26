import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
<<<<<<< Updated upstream
import { MapComponent } from './components/map/map.component';
import { CardsEmployeeComponent } from './components/cards-employee/cards-employee.component';
=======
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
>>>>>>> Stashed changes

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    MapComponent,
    CardsEmployeeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
