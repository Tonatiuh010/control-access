import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  API_URL = 'https://controlaccessapp.azurewebsites.net/api/'
  constructor(private http: HttpClient) { }

  getEmployees(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Employee')
  }

  getCatalog(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Catalog/Assets')
  }

  postEmployee(data: any): Observable<any> {
    return this.http.post<any>(this.API_URL + 'Employee', data)
  }

  updateEmployee(data: any): Observable<any> {
    return this.http.post<any>(this.API_URL + 'Employee', data)
  }

  disableEmployee(data: any): Observable<any> {
    return this.http.post<any>(this.API_URL + 'Employee/downEmployee', data)
  }

  getCards(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Card')
  }

  getDevices(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Device')
  }

  getChecks(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Check')
  }

}
