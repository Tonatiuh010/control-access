import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {
  API_URL = 'https://controlaccess20220725234915.azurewebsites.net/api/'
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

  getCards(): Observable<any> {
    return this.http.get<any>(this.API_URL + 'Card')
  }
}
