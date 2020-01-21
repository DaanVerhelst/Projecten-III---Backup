import { Component, OnInit, Input, EventEmitter, Output, OnChanges, SimpleChanges } from '@angular/core';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { faPlus, faCircle, faArrowRight, faArrowLeft } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-atelier-list',
  templateUrl: './atelier-list.component.html',
  styleUrls: ['./atelier-list.component.css']
})
export class AtelierListComponent implements OnInit, OnChanges {
  @Input() atelierList: IAtelier[];
  @Output() SelectionChanged: EventEmitter<IAtelier> = new EventEmitter<IAtelier>();
  @Input() selectedAtelier: IAtelier;

  protected filterdList: IAtelier[];
  protected selectedIndex: number;

  protected faPlus = faPlus;
  protected faCircle = faCircle;
  protected faArrowRight = faArrowRight;
  protected faArrowLeft = faArrowLeft;
  protected numPages: number;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    this.filterdList = this.atelierList;
    this.selectedIndex = this.selectedAtelier == null ?
                         1 : this.getPageNumber(this.selectedAtelier);
    this.numPages = Math.ceil(this.filterdList.length / 12);
  }

  ngOnInit() {}

  public getPageNumber(at: IAtelier): number {
    const indx = this.filterdList.indexOf(this.selectedAtelier);
    if (indx > 0) {
      return Math.ceil(indx / 12);
    }
    return 1;
  }

  public range(start: number, end: number): number[] {
    return (new Array(end - start + 1)).fill(undefined).map((_, i) => i + start);
  }

  public filterAtelierLijst(filter: string): void {
    this.filterdList = this.atelierList.filter(e => {
      return e.naam.toLowerCase().startsWith(filter.toLowerCase());
    });

    this.selectedIndex = 1;
    this.numPages = Math.ceil(this.filterdList.length / 12);
  }

  public reset(): void {
    this.selectedAtelier = null;
    this.SelectionChanged.emit(null);
  }

  public onClick(atelier: IAtelier) {
    this.selectedAtelier = (this.selectedAtelier === atelier) ? null : atelier;
    this.SelectionChanged.emit(this.selectedAtelier);
  }

  protected isSelected(atelier: IAtelier): boolean {
    return (atelier === this.selectedAtelier);
  }
}
