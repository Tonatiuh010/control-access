import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MapComponent } from './components/map/map.component';
import { CardsEmployeeComponent } from './components/cards-employee/cards-employee.component';
import { EntrancesDevicesComponent } from './components/entrances-devices/entrances-devices.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    data: { title: 'Dashboard' }
  },
  {
    path: 'map',
    component: MapComponent,
    data: { title: 'Map'}
  },
  {
    path: 'cards-employee',
    component: CardsEmployeeComponent,
    data: { title: 'Cards & Employees'}
  },{
    path: 'entrances-devices',
    component: EntrancesDevicesComponent,
    data: { title: 'Entrances & Devices'}
  },
  { path: '**', component: DashboardComponent },
  { path: '', component: DashboardComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
