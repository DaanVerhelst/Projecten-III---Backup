import { Component, OnInit, Input } from '@angular/core';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { Router } from '@angular/router';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {

  @Input() navList: IStringKeyStore[];
  public navConcreteList: IStringKeyStore[] = [];

  constructor(
    private router: Router
  ) { }

  ngOnInit() {
    this.buildBreadcrumbs();
  }

  buildBreadcrumbs() {
    const navTempList = [];
    this.navList.forEach(e => {
      navTempList.push(...e.value);
      this.navConcreteList.push({key: e.key, value: navTempList.slice()});
    });

  }

  navigateFromBreadcrumbs(name: string) {
    const SUB_LST = this.navConcreteList.filter(i => i.key === name);
    if (SUB_LST.length === 1) {
      this.router.navigate(SUB_LST.pop().value);
    } else {
      throw Error();
    }
  }
}
