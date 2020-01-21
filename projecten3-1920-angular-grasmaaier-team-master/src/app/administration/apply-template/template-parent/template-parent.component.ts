import { Component, OnInit } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import * as _moment from 'moment';
// @ts-ignore
import {default as _rollupMoment, Moment} from 'moment';
const moment = _rollupMoment || _moment;
import { DateServiceService } from 'src/app/shared/services/date-service.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-template-parent',
  templateUrl: './template-parent.component.html',
  styleUrls: ['./template-parent.component.css']
})
export class TemplateParentComponent implements OnInit {
  protected mode: Modes = Modes.Admin_Start;
  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Template toepassen', value: ['apply-template']}
  ];

  protected date: Moment;
  constructor(private dateServ: DateServiceService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const dt = (String)(params.date);
      if (dt === 'undefined') {
        this.date = null;
      } else {
        this.changeDate(moment(dt, 'YYYY-MM-DDTHH:mm:ss'));
      }
    });
  }

  changeDate(date: Moment): void {
    this.date = date;
    this.breadcrumb.pop();
    this.breadcrumb.push({key: 'Week van ' +
      this.dateServ.caluculateBeginningOfWeek(date).format('DD-MMMM'),
      value: ['apply-template']});
  }

  clearDate($event: any): void {

    this.date = null;
  }
}
