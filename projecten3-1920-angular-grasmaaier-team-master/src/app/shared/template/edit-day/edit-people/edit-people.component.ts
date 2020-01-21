import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SharedBreadCrumbService } from 'src/app/shared/breadcrumb/shared-bread-crumb.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { PeopleDataService } from 'src/app/administration/people/people-data.service';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { mergeMap } from 'rxjs/operators';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-edit-people',
  templateUrl: './edit-people.component.html',
  styleUrls: ['./edit-people.component.css']
})
export class EditPeopleComponent implements OnInit {
  @Input() activiteit: IAtelier;
  @Output() notify: EventEmitter<any> = new EventEmitter<any>();

  protected toeTeVoegenBegeleiders: number[] = [];
  protected toeTeVoegenClienten: number[] = [];
  protected teVerwijderenBegeleiders: number[] = [];
  protected teVerwijderenClienten: number[] = [];

  @Input() alleBegeleiders: IPersoon[];
  @Input() alleClienten: IPersoon[];
  protected filteredBegeleiders: IPersoon[];
  protected filteredClienten: IPersoon[];

  public faSearch = faSearch;

  constructor(
    private sharedBreadService: SharedBreadCrumbService,
    private router: Router,
    private route: ActivatedRoute,
    private persoonDataService: PeopleDataService
  ) { }

  ngOnInit() {
    this.filteredClienten = this.alleClienten.sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
    this.alleClienten.forEach(e => {
      if (this.activiteit.clienten && this.activiteit.clienten.find(actCl => actCl === e.id)) {
        e.selected = true;
      } else {
        e.selected = false;
      }
    });

    this.filteredBegeleiders = this.alleBegeleiders.sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
    this.alleBegeleiders.forEach(e => {
      if (this.activiteit.begeleides && this.activiteit.begeleides.find(actBeg => actBeg === e.id)) {
        e.selected = true;
      } else {
        e.selected = false;
      }
    });
  }

  save() {
    this.teVerwijderenBegeleiders.forEach(e => {
      const index = this.activiteit.begeleides.findIndex(f => f === e);
      this.activiteit.begeleides.splice(index, 1);
    });

    this.teVerwijderenClienten.forEach(e => {
      const index = this.activiteit.clienten.findIndex(f => f === e);
      this.activiteit.clienten.splice(index, 1);
    });

    this.activiteit.begeleides = this.activiteit.begeleides.concat(this.toeTeVoegenBegeleiders);
    this.activiteit.clienten = this.activiteit.clienten.concat(this.toeTeVoegenClienten);
    this.notify.emit();
  }

  cancel() {
    this.notify.emit();
  }

  getBegeleiderLst(): IPersoon[] {
    return this.filteredBegeleiders;
  }

  getClientenLst(): IPersoon[] {
    return this.filteredClienten;
  }

  handleCheckBoxBegeleider(item: IPersoon) {
    this.updateLists(item, this.activiteit.begeleides, this.teVerwijderenBegeleiders, this.toeTeVoegenBegeleiders);
  }

  handleCheckBoxClient(item: IPersoon) {
    this.updateLists(item, this.activiteit.clienten, this.teVerwijderenClienten, this.toeTeVoegenClienten);
  }

  private updateLists(item: IPersoon, allePersonen: number[], teVerwijderen: number[], toeTeVoegen: number[]) {
    if (item.selected) {
      if (allePersonen.find(e => e === item.id)) {
        teVerwijderen.push(item.id);
      } else {
        const index = toeTeVoegen.findIndex(e => e === item.id);
        toeTeVoegen.splice(index, 1);
      }
    } else {
      if (allePersonen.find(e => e === item.id)) {
        const index = teVerwijderen.findIndex(e => e === item.id);
        teVerwijderen.splice(index, 1);
      } else {
        toeTeVoegen.push(item.id);
      }
    }
    item.selected = !item.selected;
  }

  filterClienten(query: string) {
    this.filteredClienten = this.alleClienten.filter((e: IPersoon) => this.checkString(query, e))
    .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  filterBegeleiders(query: string) {
    this.filteredBegeleiders = this.alleBegeleiders.filter((e: IPersoon) => this.checkString(query, e))
    .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam));
  }

  private checkString(query: string, persoon: IPersoon): boolean {
    const voorAchter = `${persoon.voornaam.toLowerCase()} ${persoon.familienaam.toLowerCase()}`;
    const achterVoor = `${persoon.familienaam.toLowerCase()} ${persoon.voornaam.toLowerCase()}`;
    return (voorAchter.startsWith(query.toLowerCase()) || achterVoor.startsWith(query.toLowerCase()));
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
}
