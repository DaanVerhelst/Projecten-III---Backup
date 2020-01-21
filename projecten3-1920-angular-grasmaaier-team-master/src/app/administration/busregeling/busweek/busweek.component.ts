import { Component, OnInit, Input } from '@angular/core';
import { IBus } from 'src/app/data-types/IBus';
import { BusregelingDataService } from '../busregeling-data.service';

@Component({
  selector: 'app-busweek',
  templateUrl: './busweek.component.html',
  styleUrls: ['./busweek.component.css']
})
export class BusWeekComponent implements OnInit {

  @Input() weekNummer: number;
  protected busMartix: IBus[][];

  constructor(
    private dataService: BusregelingDataService
  ) { }

  ngOnInit() {
    this.dataService.getBussenInWeek$(this.weekNummer).subscribe(
      data => {
        this.busMartix = data;
    });
  }

}
