import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { DagboekCategorie } from 'src/app/data-types/DagboekCategorie';
import { map } from 'rxjs/operators';
import { Moment } from 'moment';
import { DateServiceService } from 'src/app/shared/services/date-service.service';

@Injectable({
  providedIn: 'root'
})
export class DagboekService {
  constructor(private http: HttpClient, private dateServ: DateServiceService) { }

  public getCategorieën(): Observable<DagboekCategorie[]> {
    return this.http.get<DagboekCategorie[]>(`${environment.apiUrl}/Dag/notes/categories`).pipe(
      map((data: string[]): DagboekCategorie[] => {
        return data.map(x => DagboekCategorie[x]);
      })
    );
  }

  public postNotities(date: Moment, categorie: string, comment: string): Observable<any> {
    let cat: DagboekCategorie = null;
    for (const name in DagboekCategorie) {
      if (DagboekCategorie[name] === categorie) {
        cat = DagboekCategorie[name as string];
      }
    }

    const dateTime: string = date.format('YYYY-MM-DD');
    return this.http.post(`${environment.apiUrl}//Dag/day/notes/${dateTime}` +
    `/${cat}/${comment}`, {});
  }

  public getNotities(date: string): Observable<Map<DagboekCategorie, string>> {
    return this.http.get<Map<DagboekCategorie, string>>(
      `${environment.apiUrl}/Dag/${date}/notes`
      ).pipe(
        map((data: any): Map<DagboekCategorie, string> => {
          const lijst = new Map<DagboekCategorie, string >();
          const json: any[] = data.lijstNoties;
          json.forEach(x => {
            lijst.set(DagboekCategorie[x.categorie as string], x.comment);
          });

          return lijst;
        })
    );
  }
}
