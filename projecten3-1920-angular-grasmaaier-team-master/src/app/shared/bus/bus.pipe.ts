import { IBus } from 'src/app/data-types/IBus';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bus'
})
export class BusPipe implements PipeTransform {

  transform(value: IBus[], page: number): IBus[] {
    if (value != null) {
      const numEle: number = value.length;
      if (page <= 0) {
        page = 1;
      }

      if (page > numEle % 12) {
        let start: number = value.length - 12;
        start = start < 0 ? 0 : start;
        return value.slice(start, value.length);
      }

      return value.slice(12 * (page - 1) , 12 * page);
    }
  }
}
