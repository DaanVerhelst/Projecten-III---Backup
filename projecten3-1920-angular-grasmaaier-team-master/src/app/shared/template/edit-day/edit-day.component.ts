import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { AtelierDataService } from 'src/app/shared/atelier/atelier-data.service';
import { SharedBreadCrumbService } from 'src/app/shared/breadcrumb/shared-bread-crumb.service';
import { TemplateDataService } from '../template-data.service';
import { mergeAll, map, mergeMap } from 'rxjs/operators';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { PeopleDataService } from 'src/app/administration/people/people-data.service';
import * as _moment from 'moment';
// @ts-ignore
import {default as _rollupMoment, Moment} from 'moment';
const moment = _rollupMoment || _moment;
import { DateServiceService } from '../../services/date-service.service';
import { ApplyTemplateService } from 'src/app/administration/apply-template/apply-template.service';


@Component({
  templateUrl: './edit-day.component.html',
  styleUrls: ['./edit-day.component.css']
})
export class EditDayComponent implements OnInit {
  protected weekNr: number;
  protected date: string;
  protected dagNr: number;
  protected mode: Modes = Modes.Admin;
  protected breadcrumb: IStringKeyStore[];

  protected fixedAtelierList: IAtelier[];
  protected dynamicAtelierList: IAtelier[] = [];
  public selectedAtelier: IAtelier;

  protected voegPersonentoemode = false;
  protected alleBegeleiders: IPersoon[];
  protected alleClienten: IPersoon[];

  protected weekNrUndef: boolean = isNaN(this.weekNr);
  protected weekStr: string;

  protected errorMessage: string;
  protected warnMessage: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dataService: AtelierDataService,
    private applyTempService: ApplyTemplateService,
    private dateService: DateServiceService,
    private templateDataService: TemplateDataService,
    private persoonDataService: PeopleDataService,
    protected sharedBreadService: SharedBreadCrumbService
    ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.date = (String)(params.date);
      this.weekNr = +params.weekId;
      this.dagNr = +params.dayId;
      if (!isNaN(this.weekNr )) {
        this.templateInit();
      } else {
        this.concreteInit();
      }
    });
  }

  private concreteInit(): void {
    this.makeComplexBreadCrumb();
  }

  private makeComplexBreadCrumb(): void {
    this.breadcrumb = [
      {key: 'Menu', value: ['administration']},
    ];

    const mo: Moment = moment(this.date, 'YYYY-MM-DDTHH:mm:ss');
    this.weekStr = mo.format('DD-MMMM');

    this.breadcrumb.push({key: 'week van '
      + this.dateService.caluculateBeginningOfWeek(mo).format('DD-MMMM')
      , value: ['apply-template']});

    this.templateAlgemeen();
    this.getAteliersConcrete();
  }

  private templateInit(): void {
    this.breadcrumb = [
      {key: 'Menu', value: ['administration']},
      {key: 'Template', value: ['template']},
      {key: `Week ${this.weekNr} - ${this.sharedBreadService.getDay(this.dagNr)}`, value: ['week', this.weekNr , 'day', this.dagNr]}
    ];
    this.templateAlgemeen();
    this.getAteliersTemp();
  }

  private getAteliersConcrete(): void {
    this.applyTempService.getTemplates(this.date).subscribe((val) => {
      this.buildStructure(val);
    });
  }

  private getAteliersTemp(): void {
    this.templateDataService.getAteliersOpDag$(this.weekNr, this.dagNr).subscribe(
      (data: IAtelier[]) => {
        this.buildStructure(data);
      }
    );
  }

  private templateAlgemeen(): void {
    this.persoonDataService.getBegeleiders$().subscribe(
      (begeleiders) => {
      this.alleBegeleiders = begeleiders;
    });

    this.persoonDataService.getClienten$().subscribe(
      (clienten) => {
        this.alleClienten = clienten;
      }
    );

    this.dataService.getAllAteliers$().subscribe(
      (ateliers) => {
        this.fixedAtelierList = ateliers;
      }
    );
  }

  drop(event: CdkDragDrop<IAtelier[]>) {
    // Code works for both
    if (event.previousContainer.id === 'iconList' && event.container.id !== 'iconList') {
      this.selectedAtelier = Object.assign({}, event.previousContainer.data[event.previousIndex]);
      if (event.container.id === 'vmList') {
        this.selectedAtelier.voorMnaM = 0;
        this.selectedAtelier.start = '08:00:00';
        this.selectedAtelier.eind = '10:00:00';
      } else if (event.container.id === 'nmList') {
        this.selectedAtelier.voorMnaM = 1;
        this.selectedAtelier.start = '13:30:00';
        this.selectedAtelier.eind = '15:30:00';
      }

      this.selectedAtelier.begeleides = [];
      this.selectedAtelier.clienten = [];
      this.dynamicAtelierList.push(this.selectedAtelier);
    }
  }

  // Rewrites

  opslaan() {
    if (this.date == null || this.date === 'undefined') {
      this.templateDataService.postAteliersOpDag$(this.weekNr, this.dagNr, this.dynamicAtelierList).subscribe(() =>
        this.router.navigate(['administration', 'template']),
        e => this.errorMessage = 'Er ging iets fout. Kijk eens na of alle uren kloppen.'
      );
    } else {
      this.applyTempService.postAteliersOpDag$(this.date, this.dynamicAtelierList).subscribe(() =>
        this.router.navigate(['administration', 'apply-template', this.date]),
        e => this.errorMessage = 'Er ging iets fout. Kijk eens na of alle uren kloppen.'
      );
    }
  }

  cancel() {
    if (this.date == null || this.date === 'undefined') {
      this.router.navigate(['administration', 'template']);
    } else {
      this.router.navigate(['administration', 'apply-template', this.date]);
    }
  }

  verwijderSelectedItem() {
    // Code works for both
    const index = this.dynamicAtelierList.findIndex(e => e === this.selectedAtelier);
    this.dynamicAtelierList.splice(index, 1);
    this.selectedAtelier = undefined;
  }

  voegPersonenToe() {
    this.voegPersonentoemode = true;
  }

  editItem(data: IAtelier) {
    this.selectedAtelier = data;
  }

  visualizeSwitchList(index: number) {
    const a = this.dynamicAtelierList.filter(e => e.voorMnaM === index);
    return a;
  }

  buildStructure(data: IAtelier[]) {
    // Works for both
    data.forEach(e => {
      e.naam = this.fixedAtelierList.find(p => p.atelierID === e.atelierID).naam;
      e.voorMnaM = this.beslisVoorMnaM(e);
    });
    this.dynamicAtelierList = data;
  }

  private beslisVoorMnaM(item: IAtelier): number {
    // tslint:disable-next-line: radix
    const beginHour: number[] = item.start.split(':').map(e => parseInt(e));

    if (beginHour[0] < 13) {
      return 0;
    } else {
      return 1;
    }
  }

  startUurChanged(startUur: string[]) {
    // Eh works for both I geuss
    const duur: string[] = this.getDuur(this.selectedAtelier.start, this.selectedAtelier.eind);
    this.selectedAtelier.start = startUur.join(':');
    this.selectedAtelier.eind = this.getEindUur(this.selectedAtelier.start, duur).join(':');
    this.dynamicAtelierList.forEach(e => {
      e.voorMnaM = this.beslisVoorMnaM(e);
    });
    this.checkSelectedHours();
  }

  duurChanged(duur: string[]) {
    this.selectedAtelier.eind = this.getEindUur(this.selectedAtelier.start, duur).join(':');
    this.dynamicAtelierList.forEach(e => {
      e.voorMnaM = this.beslisVoorMnaM(e);
    });
    this.checkSelectedHours();
  }

  getDuur(beginStr: string, eindStr: string): string[] {
    // tslint:disable-next-line: radix
    const beginHour: number = beginStr.split(':').map(e => parseInt(e)).reduce(
      (accumulator, currentValue, index) => accumulator + currentValue * Math.pow(60, 2 - index), 0
    );
    // tslint:disable-next-line: radix
    const endHour: number = eindStr.split(':').map(e => parseInt(e)).reduce(
      (accumulator, currentValue, index) => accumulator + currentValue * Math.pow(60, 2 - index), 0
    );

    const diff = endHour - beginHour;
    const duur: number[] = [0, 0, 0];
    duur.reduce((accumulator, _, index) => {
        duur[index] = Math.floor((diff - accumulator) / Math.pow(60, 2 - index));
        return accumulator + duur[index] * Math.pow(60, 2 - index);
    }, 0);
    return duur.map(e => {
      const s = `00${e}`;
      return s.substr(s.length - 2);
    });
  }

  private getEindUur(beginStr: string, duurStr: string[]): string[] {
    // tslint:disable-next-line: radix
    const beginHour: number = beginStr.split(':').map(e => parseInt(e)).reduce(
      (accumulator, currentValue, index) => accumulator + currentValue * Math.pow(60, 2 - index), 0
    );
    // tslint:disable-next-line: radix
    const duur: number = duurStr.map((e: string) => parseInt(e)).reduce(
      (accumulator, currentValue, index) => accumulator + currentValue * Math.pow(60, 2 - index), 0
    );

    const sum = beginHour + duur;
    const eindUur: number[] = [0, 0, 0];
    eindUur.reduce((accumulator, _, index) => {
      eindUur[index] = Math.floor((sum - accumulator) / Math.pow(60, 2 - index));
      return accumulator + eindUur[index] * Math.pow(60, 2 - index);
    }, 0);
    return eindUur.map(e => {
      const s = `00${e}`;
      return s.substr(s.length - 2);
    });
  }

  notifiedByAddPeople(event) {
    // Works for both
    this.voegPersonentoemode = false;
  }

  getBegeleiderLst(): IPersoon[] {
    // works for both
    return this.selectedAtelier.begeleides.map(
      b => this.alleBegeleiders
      .find(e => e.id === b))
      .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  getClientenLst(): IPersoon[] {
    // works for both
    return this.selectedAtelier.clienten.map(
      b => this.alleClienten
      .find(e => e.id === b))
      .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  private sortAlphabetic(a: string, b: string): number {
    // works for both
    if (a < b) {
      return -1;
    }
    if (a > b) {
      return 1;
    }
    return 0;
  }

  private checkSelectedHours() {
    // Works for both
    // tslint:disable-next-line: radix
    if (parseInt(this.selectedAtelier.eind.split(':')[0]) >= 24) {
      this.warnMessage = `${this.selectedAtelier.naam} kan niet tot na middernacht door gaan.`;
    } else {
      this.warnMessage = undefined;
    }
  }
}
