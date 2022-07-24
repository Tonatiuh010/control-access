import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AppComponent implements OnInit{
  activeLink = links[0].displayName;
  routes = links
  icon = '';
  constructor(private router: Router){}
  ngOnInit(): void {
    this.router.navigate(['/cards-employee']);
    this.activeLink = links[0].displayName;
  }

}

const links = [
  {
    displayName: 'Dashboard',
    routeName: 'dashboard',
    icon: '<i class="fa-solid fa-chart-line"></i>'
  },{
    displayName: 'Map & Recent Access',
    routeName: 'map',
    icon: '<i class="fa-solid fa-map"></i>'
  },{
    displayName: 'Employees & Cards',
    routeName: 'cards-employee',
    icon: '<i class="fa-solid fa-user-plus"></i>'
  },{
    displayName: 'Entrances & Devices',
    routeName: 'entrances-devices',
    icon: '<i class="fa-solid fa-diagram-project"></i>'
  },];
