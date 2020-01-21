import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { mergeMap, mergeAll, map } from 'rxjs/operators';
import { IDagMenu } from 'src/app/data-types/IDagMenu';
import { WeekmenuService } from './../weekmenu/weekmenu.service';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { Component, OnInit } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-dagmenu',
  templateUrl: './dagmenu.component.html',
  styleUrls: ['./dagmenu.component.css']
})
export class DagmenuComponent implements OnInit {
  protected mode: Modes = Modes.Admin_Start;
  protected breadcrumb: IStringKeyStore[] = [
    { key: 'Menu', value: ['administration'] },
    { key: 'Weekmenu', value: ['menu', 'weekmenu'] }
  ];
  protected menu: IDagMenu;
  protected menuId: number;
  protected menuForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private weekmenuService: WeekmenuService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params
      .pipe(
        mergeMap((params: Params) => {
          this.menuId = +params.id;
          return this.weekmenuService.getMenuById$(this.menuId);
        })
      )
      .subscribe(data => {
        this.menu = data;
        this.breadcrumb.push({
          key: this.menu.dagNaam,
          value: [String(this.menuId)]
        });
      });

    this.menuForm = this.fb.group({
      soep: [''],
      groente: [''],
      vlees: [''],
      supplement: ['']
    });
  }

  onSubmit() {

    if (this.menuForm.value.groente) {
      this.menu.groente = this.menuForm.value.groente;
    } else { this.menu.groente = this.menu.groente; }
    if (this.menuForm.value.soep) {
      this.menu.soep = this.menuForm.value.soep;
    } else { this.menu.soep = this.menu.soep; }
    if (this.menuForm.value.vlees) {
      this.menu.vlees = this.menuForm.value.vlees;
    } else { this.menu.vlees = this.menu.vlees; }
    if (this.menuForm.value.supplement) {
      this.menu.supplement = this.menuForm.value.supplement;
    } else { this.menu.supplement = this.menu.supplement; }

    this.weekmenuService.putMenuById$(this.menu)
      .subscribe(() => this.router.navigateByUrl('administration/menu/weekmenu'));
  }

  onCancel() {
    this.router.navigateByUrl('administration/menu/weekmenu');
  }
}
