import { Injectable } from '@angular/core';
import {Observable, Subject} from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class ResetFormService {
  private subject = new Subject<any>()
  sendResetForm(){
    this.subject.next(true);
  }

  getResetForm(){
    return this.subject.asObservable();
  }
}
