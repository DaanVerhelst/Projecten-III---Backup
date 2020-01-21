import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IDagMenu } from 'src/app/data-types/IDagMenu';

@Injectable({
  providedIn: 'root'
})
export class WeekmenuService {

  constructor(private http: HttpClient) { }

  getAllMenusOfTheWeek$() {
    return this.http.get<IDagMenu[]>(`${environment.apiUrl}/DagMenu/`);
  }

  getMenuById$(id: number) {
    return this.http.get<IDagMenu>(`${environment.apiUrl}/DagMenu/${id}`);
  }

  putMenuById$(menu: IDagMenu) {
    return this.http.put(`${environment.apiUrl}/DagMenu/${menu.id}`, menu);
  }
}
