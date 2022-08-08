import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable, Subject} from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class InformationService {
  // private subject = new Subject<any>()
  // sendResetForm(){
  //   this.subject.next(true);
  // }

  // getResetForm(){
  //   return this.subject.asObservable();
  // }

  private employee$ = new BehaviorSubject<any>({});
  selectedEmployee$ = this.employee$.asObservable();

  private showEmployee$ = new BehaviorSubject<boolean>(false);
  employeeState$ = this.showEmployee$.asObservable();

  private sendType$ = new BehaviorSubject<boolean>(false);
  employeeType$ = this.sendType$.asObservable();

  private sendCardActivation$ = new BehaviorSubject<boolean>(false);
  cardActivation$ = this.sendCardActivation$.asObservable();

  constructor() {}

  sendEmployee(employee: any) {
    this.employee$.next(employee);
  }

  sendEmployeeState(state: boolean) {
    this.showEmployee$.next(state);
  }

  sendType(state: boolean) {
    this.sendType$.next(state);
  }

  sendActivation(state: boolean) {
    this.sendCardActivation$.next(state);
  }
}
