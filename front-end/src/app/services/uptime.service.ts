import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class UptimeService {

  constructor() { }

  sendDate(date: string) {
    return moment(date).format('MM/DD/YYYY, h:mm:ss a');
  }
}
