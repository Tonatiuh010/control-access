import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {

  constructor(private http: HttpClient) { }

  getEmployees() {
    return this.http.get("https://reqres.in/api/users?page=2")
    // .subscribe(data => {
    // });
  }
}
