import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { faClock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-hour-picker',
  templateUrl: './hour-picker.component.html',
  styleUrls: ['./hour-picker.component.css']
})
export class HourPickerComponent implements OnInit {
  protected faTime = faClock;
  @Input() hour: string[];
  @Input() maximumHour: number;
  @Output() notify: EventEmitter<string[]> = new EventEmitter<string[]>();

  constructor() {}

  ngOnInit() {
  }

  onRightChanged(event: any) {
    const shouldUpdate = this.pickerBehaviour(event, 60);
    if (shouldUpdate) {
      this.updateTime(1, event.target.value);
    }
  }

  onLeftChanged(event: any) {
    const shouldUpdate = this.pickerBehaviour(event, this.maximumHour + 1);
    if (shouldUpdate) {
      this.updateTime(0, event.target.value);
    }
  }

  onRightArrowRepeat(event: any) {
        // tslint:disable-next-line: variable-name radix
    const number = parseInt(event.target.value);
    if (event.key === 'ArrowUp') {
      const value = this.modulo(number + 1, 60);
      const s = `00${ value }`;
      event.target.value = s.substr(s.length - 2);
    } else if (event.key === 'ArrowDown') {
      const value = this.modulo(number - 1, 60);
      const s = `00${ value }`;
      event.target.value = s.substr(s.length - 2);
    }
  }

  onLeftArrowRepeat(event: any) {
        // tslint:disable-next-line: variable-name radix
    const number = parseInt(event.target.value);
    if (event.key === 'ArrowUp') {
      const value = this.modulo(number + 1, this.maximumHour + 1);
      const s = `00${ value }`;
      event.target.value = s.substr(s.length - 2);
    } else if (event.key === 'ArrowDown') {
      const value = this.modulo(number - 1, this.maximumHour + 1);
      const s = `00${ value }`;
      event.target.value = s.substr(s.length - 2);
    }
  }


  private modulo(value: number, modulo: number): number {
    if (value < 0) {
      return this.modulo(value + modulo, modulo);
    } else {
      return value % modulo;
    }
  }

  private updateTime(index: number, time: string) {
    if (time === '') {
      time = '0';
    }
    this.hour[index] = time;

    const emitTime = [...this.hour];
    const s = `00${ time }`;
    emitTime[index] = s.substr(s.length - 2);
    this.notify.emit(emitTime);
  }

  private pickerBehaviour(event: any, max: number): boolean {
    if (event.target.value.length > 2) {
      event.target.value = event.target.value.substr(0, event.target.value.length - 1);
      return false;
    }


    // tslint:disable-next-line: variable-name radix
    const number = parseInt(event.target.value);

    if (number > max - 1) {
      const value = this.modulo(number, max);
      const s = `00${ value }`;
      event.target.value = s.substr(s.length - 2);
    }

    return true;
  }
}
