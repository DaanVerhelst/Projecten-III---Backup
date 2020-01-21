import { Injectable } from '@angular/core';
import { Dagen } from '../template/week/dag/dag.pipe';

@Injectable({
  providedIn: 'root'
})
export class SharedBreadCrumbService {

  constructor() { }

  getDay(index: number): string {
    return Dagen[index - 1];
  }
}
