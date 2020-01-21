import { Component, OnInit, Input, OnChanges, SimpleChanges} from '@angular/core';
import { Router } from '@angular/router';
import { TemplateDataService } from '../../template-data.service';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { Moment } from 'moment';

@Component({
  selector: 'app-dag',
  templateUrl: './dag.component.html',
  styleUrls: ['./dag.component.css']
})
export class DagComponent implements OnInit , OnChanges {

  @Input() weekNr?: number;
  @Input() dagNr?: number;
  @Input() isConcrete = false;
  @Input() atelierList: IAtelier[];
  @Input() day?: Moment;
  // protected activiteiten : IAtelier[];

  constructor(private router: Router) { }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges): void {}

  clicked(event: any) {
    if (!this.isConcrete) {
      this.router.navigate(['administration', 'template', 'week', this.weekNr , 'day', this.dagNr]);
    } else {
      this.router.navigate(['administration', 'concrete', 'day', this.day.format()]);
    }
  }
}
