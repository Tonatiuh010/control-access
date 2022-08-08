import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UptimeService {

  constructor() { }

  private time$ = new BehaviorSubject<Date>(new Date('2020-03-08 14:12:23'));
  selectedTime$ = this.time$.asObservable();

  sendDate(date: Date) {
    this.time$.next(date);
  }
}
