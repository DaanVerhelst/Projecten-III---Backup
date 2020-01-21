import { IBus } from './../../data-types/IBus';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BusDataService {
  public imageCache: IImageKeyStore[] = [];
  constructor(private http: HttpClient) { }

  getAllBussen$(): Observable<IBus[]> {
    return this.http.get<IBus[]>(`${environment.apiUrl}/Bus/`);
  }

  removeBus$(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Bus/${id}`);
  }

  getBussen$(id: number): Observable<IBus> {
    return this.http.get<IBus>(`${environment.apiUrl}/Bus/${id}`);
  }

  updateBusImage(id: number): void {
    const temp: IImageKeyStore[] = this.imageCache.filter(i => i.id === id);
    if (temp.length === 1) {
      this.http.get(`${environment.apiUrl}/Bus/${id}/Picto`, { responseType: 'blob' }).subscribe((val) => {
        const t = this.imageCache.map(i => i.id);
        const indx = t.indexOf(id);
        this.imageCache[indx] = { image: val, id};
      });
    } else {
      this.http.get(`${environment.apiUrl}/Bus/${id}/Picto`, { responseType: 'blob' }).subscribe((val) => {
        this.imageCache.push({ image: val, id});
      });
    }
  }

  getBusImage$(id: number): Observable<Blob> {
    const temp: IImageKeyStore[] = this.imageCache.filter(i => i.id === id);
    if (temp.length === 1) {
      const img = temp.pop().image;
      return Observable.create(obs => obs.next(img));
    } else {
      return this.http.get(`${environment.apiUrl}/Bus/${id}/Picto`, { responseType: 'blob' });
    }
  }

  postBus$(name: string): Observable<number> {
    const url = `${environment.apiUrl}/Bus/${name}`;
    return this.http.post(url, {}).pipe(
      map((x: IBus) => {
        if (x.busID) {
          return x.busID;
        } else {
          throw new Error('Er ging iets fout in de request');
        }
      })
    );
  }

  public postPicture$(id: number, uploadData: FormData) {
    return this.http.post(`${environment.apiUrl}/Bus/${id}/picto`, uploadData);
  }

  public updatePicture$(id: number, uploadData: FormData) {
    return this.http.put(`${environment.apiUrl}/Bus/${id}/picto`, uploadData);
  }

  putBussen$(id: number, name: string): Observable<number> {
    const url = `${environment.apiUrl}/Bus/${id}/${name}`;
    return this.http.put(url, {}).pipe(
      map(v => {
        return v as number;
      })
    );
  }
}
