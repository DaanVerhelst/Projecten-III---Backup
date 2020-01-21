import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dayFormat'
})
export class DayFormatPipe implements PipeTransform {

  transform(value: Date, args?: any): string {
    // tslint:disable-next-line: no-use-before-declare
    return `${Dagen[value.getDay() - 1]}`;
  }

}

enum Dagen {
  Maandag,
  Dinsdag,
  Woensdag,
  Donderdag,
  Vrijdag,
  Zaterdag,
  Zondag
}
