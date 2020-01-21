import { Pipe, PipeTransform } from '@angular/core';
import { IAtelier } from 'src/app/data-types/IAtelier';

@Pipe({
  name: 'atelier'
})
export class AtelierPipe implements PipeTransform {

  transform(value: IAtelier[], page: number): IAtelier[] {
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
