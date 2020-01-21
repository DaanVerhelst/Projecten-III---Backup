import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProfielFotoDataService {
  constructor(private http: HttpClient) { }
  public loadingError$ = new Subject<string>();
  public imageCache: IImageKeyStore[] = [];
Y: Observable<string>;
  getProfilePic$(id: number): Observable<File> {
    return this.http.get(`${environment.apiUrl}/Persoon/profilepic/${id}`, { responseType: 'blob'}).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }

  getProfilePicBegeleider$(id: number): Observable<File> {
    return this.http.get(`${environment.apiUrl}/Persoon/begeleider/profilepic/${id}`, { responseType: 'blob'}).pipe(
      catchError(error => {
        this.loadingError$.next(error.statusText);
        return of(null);
      })
    );
  }

  GetType$(): Observable<string> {
    return this.http.get(`${environment.apiUrl}/Persoon/Type`, {responseType: 'text'});
  }


}
