import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ApplyTemplateService } from '../apply-template.service';
import { DateServiceService } from 'src/app/shared/services/date-service.service';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IDag } from 'src/app/data-types/IDag';

import * as _moment from 'moment';
// @ts-ignore
import {default as _rollupMoment, Moment} from 'moment';
const moment = _rollupMoment || _moment;

@Component({
  selector: 'app-apply-template',
  templateUrl: './apply-template.component.html',
  styleUrls: ['./apply-template.component.css']
})
export class ApplyTemplateComponent implements OnInit {
  @Input() selectedDate: Moment;
  @Output() dateChangedChosen = new EventEmitter<boolean>();

  private beginDateMoment: Moment;
  protected week: Moment[];
  protected beginDate: string;
  protected selectedValue = 0;

  protected atelierMartix: IAtelier[][] = [[]];
  protected templateNumbers: number[];
  protected error: string;
  protected templateCopyGroup: FormGroup;

  constructor(private applyService: ApplyTemplateService,
              private dateService: DateServiceService, private fb: FormBuilder) { }

  fireChangeDate(): void {

    this.dateChangedChosen.emit(true);
  }

  ngOnInit() {
    this.beginDateMoment = this.dateService.caluculateBeginningOfWeek(this.selectedDate);
    this.beginDate = 'Week van ' + this.beginDateMoment.format('DD-MM');
    this.fillTemplateSelection();
    this.buildFormGroup();
    this.week = this.dateService.getWeekMoments(this.beginDateMoment);
    this.checkIfWeekHasTemplate(this.beginDateMoment);
  }

  private checkIfWeekHasTemplate(date: Moment): void {
    this.applyService.controleerWeekHeeftWaarde$(
      this.dateService.convertToDotnetFormat(date)
    ).subscribe((val) => {
      if (val) {
        this.hasTemplateFill(date);
      }
    }, (error) => {
      this.error = 'Er ging iets fout bij het controleren of de week al waardes had';
    });
  }

  private hasTemplateFill(date: Moment): void {
    this.applyService.getConcretTempsForDay(
      this.dateService.convertToDotnetFormat(date)
    ).subscribe((val) => {
      this.atelierMartix = val.map(v => v.ateliers);
    });
  }

  private buildFormGroup(): void {
    this.templateCopyGroup = this.fb.group(
      {templateNr: [null]}
    );
  }

  private fillTemplateSelection(): void {
    this.applyService.getTemplateNummers().subscribe(
      (val: number[]) => {
        this.selectedValue = val[0];
        this.templateNumbers = val;
      }, (error: any) => {
        this.error = 'Er ging iets fout bij het ophalen van de ateliers, probeer het later nog eens.';
      }
    );
  }

  public copyTemplate(): void {
    const val: number = this.templateCopyGroup.get('templateNr').value;
    if (val != null) {
      const dateTime: string = this.dateService.convertToDotnetFormat(this.beginDateMoment);
      this.applyService.cloneTemplateInWeek$(dateTime, val).subscribe(
        (at: IDag[]) => {
          this.atelierMartix = this.convertToAtelierArray(at);
        }, (error: any) => {
          this.error = 'Er ging iets fout bij het clonen van de template';
        });
    }
  }

  private convertToAtelierArray(dagen: IDag[]): IAtelier[][] {
    const res = [];

    for (const dt of this.week) {
      const dag = dagen.filter(d => d.date === dt.format());
      if (dag.length === 0) {
        res.push([]);
      } else {
        res.push(dag.pop().ateliers);
      }
    }

    return res;
  }
}
