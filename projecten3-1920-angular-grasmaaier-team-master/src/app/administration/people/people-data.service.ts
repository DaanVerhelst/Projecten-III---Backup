import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { Observable, Subject, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { catchError, concatAll } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PeopleDataService {

  public loadingError$ = new Subject<string>();
  public token: string;
  // tslint:disable-next-line: variable-name
  private readonly _tokenKey = 'currentUser';
  constructor(private http: HttpClient) {
    this.token = localStorage.getItem(this._tokenKey);
   }

  // Get
  getClienten$(): Observable<IPersoon[]> {
    return this.http.get<IPersoon[]>(`${environment.apiUrl}/Persoon/clienten`).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }

  getBegeleiders$(): Observable<IPersoon[]> {
    return this.http.get<IPersoon[]>(`${environment.apiUrl}/Persoon/begeleiders`).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }

  getPersoon$(id: number): Observable<IPersoon> {
    return this.http.get<IPersoon>(`${environment.apiUrl}/Persoon/${id}`).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }

  getProfilePic$(id: number): Observable<File> {
    return this.http.get(`${environment.apiUrl}/Persoon/${id}/profilepic/`, { responseType: 'blob'}).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }
  getProfilePicBegeleider$(id: number): Observable<File> {
    return this.http.get(`${environment.apiUrl}/Persoon/begeleider/profilepic//${id}`, { responseType: 'blob'}).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }
  // posts
  postPersoon$(pers: IPersoon): Observable<IPersoon> {
    return this.http.post<IPersoon>(`${environment.apiUrl}/Persoon/`, pers);
  }

  postPersoonB$(pers: IPersoon): Observable<IPersoon> {

    return this.http.post<IPersoon>(`${environment.apiUrl}/Persoon/begeleider`, pers);
  }

  postPicture$(id: any, uploadData: FormData) {
    return this.http.post(`${environment.apiUrl}/Persoon/${id}/profilepic`, uploadData);
  }

  postPictureB$(id: any, uploadData: FormData) {
    console.log(uploadData);
    return this.http.put(`${environment.apiUrl}/Persoon/begeleider/${id}/profilepic`, uploadData);
  }

  // puts

  putPersoon$(id: number, pers: IPersoon): Observable<IPersoon> {
    return this.http.put<IPersoon>(`${environment.apiUrl}/Persoon/${id}`, pers);
  }

  putPersoonB$(id: number, pers: IPersoon): Observable<IPersoon> {
    return this.http.put<IPersoon>(`${environment.apiUrl}/Persoon/${id}`, pers);
  }

  putPicture$() {

  }
  // delete

  deletePersoon$(id: number) {
    return this.http.delete<IPersoon>(`${environment.apiUrl}/Persoon/${id}`);
  }

  deletePersoonBegeleider$(id: number) {
    return this.http.delete<IPersoon>(`${environment.apiUrl}/Persoon/begeleider/${id}`);
  }

  // register

  registerBegeleider$(isAdmin: boolean, pers: IPersoon) {
    return this.http.post<IPersoon>(`${environment.apiUrl}/Gebruiker/begeleider/registreer/${isAdmin}`, pers);
  }



}
