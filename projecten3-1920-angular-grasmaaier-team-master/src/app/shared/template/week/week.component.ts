import { Component, OnInit, Input } from '@angular/core';
import { TemplateDataService } from '../template-data.service';
import { IAtelier } from 'src/app/data-types/IAtelier';

@Component({
  selector: 'app-week',
  templateUrl: './week.component.html',
  styleUrls: ['./week.component.css']
})
export class WeekComponent implements OnInit {

  @Input() weekNummer: number;
  protected atelierMartix: IAtelier[][];

  constructor(
    private dataService: TemplateDataService
  ) { }

  ngOnInit() {
    this.dataService.getAteliersInWeek$(this.weekNummer).subscribe(
      data => {
        this.atelierMartix = data;
    });
  }

}
