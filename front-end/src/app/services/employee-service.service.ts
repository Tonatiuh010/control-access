import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {
  API_URL = 'https://controlaccess20220725234915.azurewebsites.net/api/'
  constructor(private http: HttpClient) { }

  getEmployees() {
    return this.http.get(this.API_URL + 'Employee')
  }
}
