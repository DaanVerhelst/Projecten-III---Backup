import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormat'
})
export class DateFormatPipe implements PipeTransform {

  transform(value: Date, args?: any): string {
    // tslint:disable-next-line: no-use-before-declare
    return `${value.getUTCDate()} ${Maanden[value.getMonth()]} ${value.getFullYear()}`;
  }

}

enum Maanden {
  Januari,
  Februari,
  Maart,
  April,
  Mei,
  Juni,
  Juli,
  Augustus,
  September,
  Oktober,
  November,
  December
}
