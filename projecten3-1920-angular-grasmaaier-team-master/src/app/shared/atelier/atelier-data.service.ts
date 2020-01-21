import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { environment } from 'src/environments/environment';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AtelierDataService {
  public imageCache: IImageKeyStore[] = [];
  constructor(private http: HttpClient) { }

  getAllAteliers$(): Observable<IAtelier[]> {
    return this.http.get<IAtelier[]>(`${environment.apiUrl}/Atelier/`);
  }

  removeAtelier$(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Atelier/${id}`);
  }

  getAtelier$(id: number): Observable<IAtelier> {
    return this.http.get<IAtelier>(`${environment.apiUrl}/Atelier/${id}`);
  }

  getNames$(): Observable<string[]> {
    return this.getAllAteliers$().pipe(
      map((a: IAtelier[]) => a.map(at => at.naam))
    );
  }

  updateAtelierImage(id: number): void {
    const temp: IImageKeyStore[] = this.imageCache.filter(i => i.id === id);
    if (temp.length === 1) {
      this.http.get(`${environment.apiUrl}/Atelier/${id}/Picto`, { responseType: 'blob' }).subscribe((val) => {
        const t = this.imageCache.map(i => i.id);
        const indx = t.indexOf(id);
        this.imageCache[indx] = { image: val, id};
      });
    } else {
      this.http.get(`${environment.apiUrl}/Atelier/${id}/Picto`, { responseType: 'blob' }).subscribe((val) => {
        this.imageCache.push({ image: val, id});
      });
    }
  }

  getAtelierImage$(id: number): Observable<Blob> {
    const temp: IImageKeyStore[] = this.imageCache.filter(i => i.id === id);
    if (temp.length === 1) {
      const img = temp.pop().image;
      return Observable.create(obs => obs.next(img));
    } else {
      return this.http.get(`${environment.apiUrl}/Atelier/${id}/Picto`, { responseType: 'blob' });
    }
  }

  postAtelier$(name: string): Observable<number> {
    const url = `${environment.apiUrl}/Atelier/${name}`;
    return this.http.post(url, {}).pipe(
      map((x: IAtelier) => {
        if (x.atelierID) {
          return x.atelierID;
        } else {
          throw new Error('Er ging iets fout in de request');
        }
      })
    );
  }

  public postPicture$(id: number, uploadData: FormData) {
    return this.http.post(`${environment.apiUrl}/Atelier/${id}/picto`, uploadData);
  }

  public updatePicture$(id: number, uploadData: FormData) {
    return this.http.put(`${environment.apiUrl}/Atelier/${id}/picto`, uploadData);
  }

  putAtelier$(id: number, name: string): Observable<number> {
    const url = `${environment.apiUrl}/Atelier/${id}/${name}`;
    return this.http.put(url, {}).pipe(
      map(v => {
        return v as number;
      })
    );
  }
}
