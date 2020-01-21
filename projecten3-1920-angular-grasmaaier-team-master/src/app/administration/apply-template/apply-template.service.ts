import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { IDag } from 'src/app/data-types/IDag';
import { map } from 'rxjs/operators';
import { IDagAtelierTemp } from 'src/app/data-types/IDagAtelierTemp';
import { IDagTemp } from 'src/app/data-types/IDagTemp';

@Injectable({
  providedIn: 'root'
})
export class ApplyTemplateService {
  constructor(private http: HttpClient) { }

  public getTemplateNummers(): Observable<number[]> {
    return this.http.get<number[]>(`${environment.apiUrl}/Template/week`);
  }

  public getConcretTempsForDay(dateTime: string): Observable<IDag[]> {
    return this.http.get<IDag[]>(`${environment.apiUrl}/Dag/week/${dateTime}`);
  }

  public cloneTemplateInWeek$(dateTime: string, weekNr: number): Observable<IDag[]> {
    return this.http.post<IDag[]>(`${environment.apiUrl}/Dag/concrete/`
    + `${dateTime}/${weekNr}`, {});
  }

  public controleerWeekHeeftWaarde$(dateTime: string): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/Dag/HasTemplate/${dateTime}`);
  }

  public getTemplates(dateTime: string): Observable<IAtelier[]> {
  return this.http.get<IDagTemp>(`${environment.apiUrl}/Dag/day/${dateTime}`)
    .pipe( map( (data: IDagTemp) => {
      return data.ateliers.map((a: IDagAtelierTemp): IAtelier => {
        return {
          atelierID: a.atelierID,
          begeleides: a.begeleiders.map(b => b.id),
          clienten: a.clienten.map(c => c.id),
          eind: a.end,
          start: a.start,
          naam: a.naam
        };
      });
    })
  );
}

postAteliersOpDag$(dag: string, ateliers: IAtelier[]): Observable<any> {
  return this.http.post<any>(
    `${environment.apiUrl}/Dag/concrete/${dag}`, ateliers.map(element => {
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
}
