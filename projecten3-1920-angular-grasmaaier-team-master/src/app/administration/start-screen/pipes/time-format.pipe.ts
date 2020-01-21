import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeFormat'
})
export class TimeFormatPipe implements PipeTransform {

  transform(value: Date, args?: any): string {
    return `${value.toLocaleTimeString()}`;
  }

}
