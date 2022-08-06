import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private counter = 0;
  private spinner$ = new BehaviorSubject<boolean>(false);
  constructor() { }

  getSpinnerObserver(): Observable<boolean>{
    return this.spinner$.asObservable();
  }

  requestStarted(): void{
    if(++this.counter === 1){
      this.spinner$.next(true);
    }
  }

  requestEnded(): void{
    if(this.counter === 0 || --this.counter === 0){
      this.spinner$.next(false);
    }
  }

  resetSpinner(): void{
    this.counter = 0;
    this.spinner$.next(false);
  }
}
