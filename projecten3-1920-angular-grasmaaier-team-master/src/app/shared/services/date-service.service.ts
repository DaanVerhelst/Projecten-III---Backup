import { Injectable } from '@angular/core';
import * as _moment from 'moment';
// :)
// @ts-ignore
import {default as _rollupMoment, Moment} from 'moment';
const moment = _rollupMoment || _moment;

@Injectable({
  providedIn: 'root'
})
export class DateServiceService {

  constructor() { }

  public caluculateBeginningOfWeek(date: Moment): Moment {
    const weekday: number = date.weekday();
    const diff = 1 - weekday;
    return date.add(diff, 'days');
  }

  public convertToDotnetFormat(date: Moment): string {
    return date.format('YYYY-MM-DDTHH:mm:ss.000') + 'Z';
  }

  public getWeekMoments(date: Moment): Moment[] {
    const start: Moment = this.caluculateBeginningOfWeek(date);
    const res: string[] = [start.format()];

    for (let i = 1; i < 5; i++) {
      res.push(start.add(1, 'days').format());
    }

    return res.map(v => moment(v));
  }
}
