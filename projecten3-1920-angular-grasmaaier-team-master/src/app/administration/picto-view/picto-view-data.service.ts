import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';

@Injectable({
  providedIn: 'root'
})
export class PictoViewDataService {
  constructor(private http: HttpClient) {
    this.token = localStorage.getItem(this._tokenKey);
  }
  public imageCache: IImageKeyStore[] = [];
  public loadingError$ = new Subject<string>();
  public token: string;
  private myDate = new Date();

  // tslint:disable-next-line: variable-name
  private readonly _tokenKey = 'currentUser';
  private dateNow = new Date();
  private dateToday = new Date(
    this.dateNow.getFullYear(),
    this.dateNow.getMonth(),
    this.dateNow.getDate()
  );
  private dateSunday = new Date(
    this.dateToday.getTime() - this.dateToday.getDay() * 24 * 3600 * 1000
  );

  getAteliersByDayByClient$(id: number): Observable<any[]> {
    return this.http
      .get<any[]>(
        `${environment.apiUrl}/Dag/week/
    ${this.myDate.getFullYear() +
      '-' +
      (this.myDate.getMonth() + 1) +
      '-' +
      this.myDate.getUTCDate()}/client/${id}`
      )
      .pipe(
        catchError(error => {
          this.loadingError$.next(error.statusText);
          return of(null);
        })
      );
  }
  voegNotitieToeZaterdag(id: number, note: string) {
    return this.http.post<any>(
      `${environment.apiUrl}/Dag/weekend/notes/${this.dateSunday.getFullYear() +
        '-' +
        (this.dateSunday.getMonth() + 1) +
        '-' +
        (this.dateSunday.getUTCDate() - 1)}/${id}/${note}`,
      note
    );
  }
  voegNotitieToeZondag(id: number, note: string) {
    return this.http.post<any>(
      `${environment.apiUrl}/Dag/weekend/notes/${this.dateSunday.getFullYear() +
        '-' +
        (this.dateSunday.getMonth() + 1) +
        '-' +
        this.dateSunday.getUTCDate()}/${id}/${note}`,
      note
    );
  }

  getalles$(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/Dag/week/Test`).pipe();
  }
}
