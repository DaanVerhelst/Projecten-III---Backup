import { IDagMenu } from '../../../data-types/IDagMenu';
import { WeekmenuService } from './weekmenu.service';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { Component, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-weekmenu',
  templateUrl: './weekmenu.component.html',
  styleUrls: ['./weekmenu.component.css']
})
export class WeekmenuComponent implements OnInit {
  protected mode: Modes = Modes.Admin_Start;
  protected errorString = '';
  protected breadcrumb: IStringKeyStore[] = [
    { key: 'Menu', value: ['administration'] },
    { key: 'Weekmenu', value: ['weekmenu'] }
  ];

   menuList: IDagMenu[];

  constructor(private menuService: WeekmenuService, private router: Router) {
  }

  ngOnInit() {
    this.menuList = [];
    this.menuService.getAllMenusOfTheWeek$().subscribe((val) => {
      val.sort((a, b) => a.dagNr - b.dagNr);
      this.menuList = val;

    }, error => {
      this.errorString = error;
    });

    // this.buildPage();
  }

  navigateToDagMenu(id) {
    this.router.navigate(['administration', 'menu', 'dagmenu', id]);
  }

}
