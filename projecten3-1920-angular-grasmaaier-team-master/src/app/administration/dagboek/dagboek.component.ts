import { Component, OnInit } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { DagboekService } from './dagboek.service';
import { DagboekCategorie } from 'src/app/data-types/DagboekCategorie';

import * as _moment from 'moment';
// @ts-ignore
import {default as _rollupMoment, Moment} from 'moment';
import { DateServiceService } from 'src/app/shared/services/date-service.service';
const moment = _rollupMoment || _moment;
@Component({
  selector: 'app-dagboek',
  templateUrl: './dagboek.component.html',
  styleUrls: ['./dagboek.component.css']
})
export class DagboekComponent implements OnInit {
  protected mode: Modes = Modes.Admin_Start;
  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Dagboek', value: ['dagboek']}
  ];

  protected cate: DagboekCategorie[] = [];
  protected selected: DagboekCategorie = null;
  protected selectedComment: string = null;
  protected selectedDate: Moment = null;
  protected notities: Map<DagboekCategorie, string>;

  constructor(
    private dagboekService: DagboekService,
    private dateService: DateServiceService
  ) { }

  ngOnInit() {
    this.notities = null;
    this.dagboekService.getCategorieën().subscribe(val => {
      this.cate = val;
      this.selected = val[0];
    });
  }

  public changeDate(date: Moment): void {
    this.notities = null;
    this.selectedDate = date;

    if (this.selectedDate != null) {
      this.dagboekService.getNotities(
        this.dateService.convertToDotnetFormat(this.selectedDate)
      ).subscribe((val) => {

        if (this.selected != null) {
          if (val.has(this.selected)) {
            this.selectedComment = val.get(this.selected);
          } else {
            this.selectedComment = null;
          }
        }

        this.notities = val;
      });

    }

    this.changeBreadcrumb();
  }

  public changeBreadcrumb() {
    this.breadcrumb.pop();

    if (this.selectedDate == null) {
      this.breadcrumb.push({key: 'Dagboek', value: ['dagboek']});
    } else {
      this.breadcrumb.push({key: `Dagboek op ${this.selectedDate.format('DD-MMMM')}`, value: ['dagboek']});
    }
  }

  public catchChange(data: any) {
    if (data.cat != null) {
      const cate: DagboekCategorie = data.cat;
      const comment: string = data.comment;

      this.notities.set(cate, comment);

      this.selected = cate;
      this.selectedComment = comment;
    }
  }

  public changeSelected(cat: DagboekCategorie): void {
    if (this.selected !== cat) {
      this.selected = cat;
      if (this.notities.has(this.selected)) {
        this.selectedComment = this.notities.get(this.selected);
      } else {
        this.selectedComment = null;
      }
    }
  }
}
