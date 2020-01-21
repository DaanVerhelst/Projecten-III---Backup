import { Component, OnInit } from '@angular/core';
import { PeopleDataService } from '../people/people-data.service';
import { Observable } from 'rxjs';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-picto-view',
  templateUrl: './picto-view.component.html',
  styleUrls: ['./picto-view.component.css']
})
export class PictoViewComponent implements OnInit {

  clients$: Observable<IPersoon[]>;

  selectedClient: IPersoon;

  protected filteredClienten: IPersoon[];

  public faSearch = faSearch;

  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Client', value: ['clients']}
  ];

  constructor(private peopleDataService: PeopleDataService) { }

  ngOnInit() {
    this.clients$ = this.peopleDataService.getClienten$();
    this.clients$.subscribe( e => this.filteredClienten = e.sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam)));

  }


  setClient(client: IPersoon) {
    this.selectedClient = client;

    }

  filterClienten(query: string) {
      this.clients$.subscribe(f => this.filteredClienten = f.filter((e: IPersoon) => this.checkString(query, e))
      .sort((a, b) => this.sortAlphabetic(a.voornaam, b.voornaam)));
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
