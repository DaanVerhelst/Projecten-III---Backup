import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dagPipe'
})
export class DagPipe implements PipeTransform {

  transform(value: number, args?: any): string {
    // tslint:disable-next-line: no-use-before-declare
    return Dagen[value];
  }

}

export enum Dagen {
  Maandag,
  Dinsdag,
  Woensdag,
  Donderdag,
  Vrijdag,
  Zaterdag,
  Zondag
}
