import { IBus } from './../../data-types/IBus';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BusregelingDataService {

  constructor(private http: HttpClient) { }

  getBussenOpDag$(weeknr: number, dagnr: number): Observable<IBus[]> {
    return this.http.get<IBus[]>(`${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}`)
    .pipe(
      map( (data: any) => {
        const modal: IBus[] = [];
        // tslint:disable-next-line: forin
        for (const key in data.templateBussen) {
          const subset = data.templateBussen[key];
          modal.push({
            busID: subset.atelierInfo.atelierID,
            begeleiders: subset.begeleiders,
            clienten: subset.clients,
            start: subset.atelierInfo.start,
            eind: subset.atelierInfo.eind,
          });
        }
        return modal;
      })
    );
  }

  postBussenOpDag$(weeknr: number, dagnr: number, bussen: IBus[]): Observable<any> {
    return this.http.post<any>(
      `${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}/create`, bussen.map(element => {
        const obj = {
          busInfo: {
            id: element.busID,
            start: element.start,
            eind: element.eind,
          },
          clients: element.clienten,
          begeleiders: element.begeleiders
        };
        return obj;
      })
    );
  }

  resetBussenOpDag$(weeknr: number, dagnr: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Template/dag/${dagnr}/week/${weeknr}/delete`);
  }

  getBussenInWeek$(weeknr: number): Observable<IBus[][]> {
    return this.http.get<IBus[][]>(`${environment.apiUrl}/Template/busweek/${weeknr}`);
  }
}
