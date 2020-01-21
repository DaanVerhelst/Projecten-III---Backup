import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { PictoViewDataService } from '../../picto-view-data.service';
import { IDag } from 'src/app/data-types/IDag';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { days } from 'src/app/data-types/Weekdays';


@Component({
  selector: 'app-picto-day',
  templateUrl: './picto-day.component.html',
  styleUrls: ['./picto-day.component.css']
})
export class PictoDayComponent implements OnInit {

  @Input() dayName: string;
  @Input() date: Date; // TODO: change by moment once merged with michiel
  @Input() dayNumber: number;
  @Input() atelier: Observable<IDag[]>;



  isLoaded: boolean;

  dag: IDag;
  dagen: IDag[];
  vm: number[];
  nm: number[];
  atelier$: IAtelier[];
  vm$: IAtelier[];
  nm$: IAtelier[];
  constructor() {
    this.isLoaded = false;

  }

  ngOnInit() {

    this.atelier.subscribe(d => {
      this.isLoaded = true;
      this.dag = d.filter(s => (this.dayNumber + 1) === new Date(s.date).getDay())[0];

      if (!!!this.dag) {
        return;
      }

      this.atelier$ = this.dag.ateliers;


      // tslint:disable-next-line: radix
      this.vm$ = this.dag.ateliers.filter(f => parseInt(f.start.substr(0, 2)) < 12);

      // tslint:disable-next-line: radix
      this.nm$ = this.dag.ateliers.filter(f => parseInt(f.start.substr(0, 2)) >= 12);
    });


  }




}
