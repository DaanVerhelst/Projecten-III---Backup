import { IBus } from 'src/app/data-types/IBus';
import { Component, OnInit, Input, EventEmitter, Output, OnChanges, SimpleChanges } from '@angular/core';
import { faPlus, faCircle, faArrowRight, faArrowLeft } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-bus-list',
  templateUrl: './bus-list.component.html',
  styleUrls: ['./bus-list.component.css']
})
export class BusListComponent implements OnInit, OnChanges {
  @Input() busList: IBus[];
  @Output() SelectionChanged: EventEmitter<IBus> = new EventEmitter<IBus>();
  @Input() selectedBus: IBus;

  protected filterdList: IBus[];
  protected selectedIndex: number;

  protected faPlus = faPlus;
  protected faCircle = faCircle;
  protected faArrowRight = faArrowRight;
  protected faArrowLeft = faArrowLeft;
  protected numPages: number;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    this.filterdList = this.busList;
    this.selectedIndex = this.selectedBus == null ?
                         1 : this.getPageNumber(this.selectedBus);
    this.numPages = Math.ceil(this.filterdList.length / 12);
  }

  ngOnInit() {
  }

  public getPageNumber(at: IBus): number {
    const indx = this.filterdList.indexOf(this.selectedBus);
    if (indx > 0) {
      return Math.ceil(indx / 12);
    }
    return 1;
  }

  public range(start: number, end: number): number[] {
    return (new Array(end - start + 1)).fill(undefined).map((_, i) => i + start);
  }

  public filterBusLijst(filter: string): void {
    this.filterdList = this.busList.filter(e => {
      return e.naam.toLowerCase().startsWith(filter.toLowerCase());
    });

    this.selectedIndex = 1;
    this.numPages = Math.ceil(this.filterdList.length / 12);
  }

  public reset(): void {
    this.selectedBus = null;
    this.SelectionChanged.emit(null);
  }

  public onClick(bus: IBus) {
    this.selectedBus = (this.selectedBus === bus) ? null : bus;
    this.SelectionChanged.emit(this.selectedBus);
  }

  protected isSelected(bus: IBus): boolean {

    return (bus === this.selectedBus);
  }
}
