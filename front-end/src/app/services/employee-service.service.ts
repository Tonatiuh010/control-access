import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Welcome } from '../components/cards-employee/employee-table/employee-table.component';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {
  API_URL = 'https://controlaccess20220725234915.azurewebsites.net/api/'
  constructor(private http: HttpClient) { }

  getEmployees(): Observable<Welcome> {
    return this.http.get<Welcome>(this.API_URL + 'Employee')
  }
}
