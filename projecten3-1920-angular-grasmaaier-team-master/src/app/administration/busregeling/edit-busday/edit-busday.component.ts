import { BusregelingDataService } from './../busregeling-data.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { SharedBreadCrumbService } from 'src/app/shared/breadcrumb/shared-bread-crumb.service';
import { mergeAll, map, mergeMap } from 'rxjs/operators';
import { PeopleDataService } from '../../people/people-data.service';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { BusDataService } from 'src/app/shared/bus/bus-data.service';
import { IBus } from 'src/app/data-types/IBus';


@Component({
  templateUrl: './edit-busday.component.html',
  styleUrls: ['./edit-busday.component.css']
})
export class EditBusDayComponent implements OnInit {
  protected weekNr: number;
  protected dagNr: number;
  protected mode: Modes = Modes.Admin;
  protected breadcrumb: IStringKeyStore[];

  protected fixedBusList: IBus[];
  protected dynamicBusList: IBus[] = [];
  public selectedBus: IBus;

  protected voegPersonentoemode = false;
  protected alleBegeleiders: IPersoon[];
  protected alleClienten: IPersoon[];

  protected errorMessage: string;
  protected warnMessage: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dataService: BusDataService,
    private busregelingDataService: BusregelingDataService,
    private persoonDataService: PeopleDataService,
    protected sharedBreadService: SharedBreadCrumbService
    ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.weekNr = +params.weekId;
      this.dagNr = +params.dayId;
      this.breadcrumb = [
        {key: 'Menu', value: ['administration']},
        {key: 'Busregeling', value: ['busregeling']},
        {key: `Week ${this.weekNr} - ${this.sharedBreadService.getDay(this.dagNr)}`, value: ['week', this.weekNr , 'day', this.dagNr]}
      ];
      this.persoonDataService.getBegeleiders$().pipe(
        mergeMap(begeleiders => {
          this.alleBegeleiders = begeleiders;
          return this.persoonDataService.getClienten$();
        })
      ).subscribe(clienten => {
        this.alleClienten = clienten;

        this.dataService.getAllBussen$().pipe( map( bussen => {
          this.fixedBusList = bussen;
          return this.busregelingDataService.getBussenOpDag$(this.weekNr, this.dagNr);
        }), mergeAll()).subscribe((data: IBus[]) => {
          this.buildStructure(data);
        });
      });
   });
  }

  drop(event: CdkDragDrop<IBus[]>) {
    if (event.previousContainer.id === 'iconList' && event.container.id !== 'iconList') {
      this.selectedBus = Object.assign({}, event.previousContainer.data[event.previousIndex]);
      if (event.container.id === 'vmList') {
        this.selectedBus.voorMnaM = 0;
        this.selectedBus.start = '08:00:00';
        this.selectedBus.eind = '10:00:00';
      } else if (event.container.id === 'nmList') {
        this.selectedBus.voorMnaM = 1;
        this.selectedBus.start = '13:30:00';
        this.selectedBus.eind = '15:30:00';
      }

      this.selectedBus.begeleiders = [];
      this.selectedBus.clienten = [];
      this.dynamicBusList.push(this.selectedBus);
    }
  }

  opslaan() {
    this.busregelingDataService.postBussenOpDag$(this.weekNr, this.dagNr, this.dynamicBusList).subscribe(() =>
      this.router.navigate(['administration', 'busregeling']),
      e => this.errorMessage = 'Er ging iets fout. Kijk eens na of alle uren kloppen.'
    );
  }

  cancel() {
    this.router.navigate(['administration', 'busregeling']);
  }

  verwijderSelectedItem() {
    const index = this.dynamicBusList.findIndex(e => e === this.selectedBus);
    this.dynamicBusList.splice(index, 1);
    this.selectedBus = undefined;
  }

  voegPersonenToe() {
    this.voegPersonentoemode = true;
  }

  editItem(data: IBus) {
    this.selectedBus = data;
  }

  visualizeSwitchList(index: number) {
    const a = this.dynamicBusList.filter(e => e.voorMnaM === index);
    return a;
  }

  buildStructure(data: IBus[]) {
    data.forEach(e => {
      e.naam = this.fixedBusList.find(p => p.busID === e.busID).naam;
      e.voorMnaM = this.beslisVoorMnaM(e);
    });
    this.dynamicBusList = data;
  }

  private beslisVoorMnaM(item: IBus): number {
    // tslint:disable-next-line: radix
    const beginHour: number[] = item.start.split(':').map(e => parseInt(e));

    if (beginHour[0] < 13) {
      return 0;
    } else {
      return 1;
    }
  }

  startUurChanged(startUur: string[]) {
    const duur: string[] = this.getDuur(this.selectedBus.start, this.selectedBus.eind);
    this.selectedBus.start = startUur.join(':');
    this.selectedBus.eind = this.getEindUur(this.selectedBus.start, duur).join(':');
    this.dynamicBusList.forEach(e => {
      e.voorMnaM = this.beslisVoorMnaM(e);
    });
    this.checkSelectedHours();
  }

  duurChanged(duur: string[]) {
    this.selectedBus.eind = this.getEindUur(this.selectedBus.start, duur).join(':');
    this.dynamicBusList.forEach(e => {
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
    this.voegPersonentoemode = false;
  }

  getBegeleiderLst(): IPersoon[] {
    return this.selectedBus.begeleiders.map(
      b => this.alleBegeleiders
      .find(e => e.id === b))
      .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  getClientenLst(): IPersoon[] {
    return this.selectedBus.clienten.map(
      b => this.alleClienten
      .find(e => e.id === b))
      .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  private sortAlphabetic(a: string, b: string): number {
    if (a < b) {
      return -1;
    }
    if (a > b) {
      return 1;
    }
    return 0;
  }

  private checkSelectedHours() {
    // tslint:disable-next-line: radix
    if (parseInt(this.selectedBus.eind.split(':')[0]) >= 24) {
      this.warnMessage = `${this.selectedBus.naam} kan niet tot na middernacht door gaan.`;
    } else {
      this.warnMessage = undefined;
    }
  }
}
