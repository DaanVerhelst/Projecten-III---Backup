import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TemplateDataService {

  constructor(private http: HttpClient) { }

  getAteliersOpDag$(weeknr: number, dagnr: number): Observable<IAtelier[]> {
    return this.http.get<IAtelier[]>(`${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}`)
    .pipe(
      map( (data: any) => {
        const modal: IAtelier[] = [];
        // tslint:disable-next-line: forin
        for (const key in data.templateActiviteiten) {
          const subset = data.templateActiviteiten[key];
          modal.push({
            atelierID: subset.atelierInfo.atelierID,
            begeleides: subset.begeleiders,
            clienten: subset.clients,
            start: subset.atelierInfo.start,
            eind: subset.atelierInfo.eind,
          });
        }
        return modal;
      })
    );
  }

  postAteliersOpDag$(weeknr: number, dagnr: number, ateliers: IAtelier[]): Observable<any> {
    return this.http.post<any>(
      `${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}/create`, ateliers.map(element => {
        const obj = {
          atelierInfo: {
            atelierID: element.atelierID,
            start: element.start,
            eind: element.eind,
          },
          clients: element.clienten,
          begeleiders: element.begeleides
        };
        return obj;
      })
    );
  }

  resetAteliersOpDag$(weeknr: number, dagnr: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}/delete`);
  }

  getAteliersInWeek$(weeknr: number): Observable<IAtelier[][]> {
    return this.http.get<IAtelier[][]>(`${environment.apiUrl}/Template/week/${weeknr}`);
  }
}
