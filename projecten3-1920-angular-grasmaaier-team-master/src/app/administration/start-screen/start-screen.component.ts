import { BehaviorSubject } from 'rxjs';
import { Component, OnInit, Renderer2, OnDestroy } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { Router } from '@angular/router';
import { element, by } from 'protractor';
import { ProfielFotoDataService } from 'src/app/shared/profile/picture/profielfoto-data.service';

declare function __weatherwidget_init(): any;

@Component({
  templateUrl: './start-screen.component.html',
  styleUrls: ['./start-screen.component.css']
})
export class StartScreenComponent implements OnInit, OnDestroy {
  constructor(
    private renderer: Renderer2,
    private router: Router,
    // tslint:disable-next-line: variable-name
    private _ProfielFotoDataService: ProfielFotoDataService
    ) {
    this.renderer.addClass(document.body, 'admin-background');
  }
  protected mode: Modes = Modes.Admin_Start;

  public isButtonVisible = true;

  ngOnInit() {
  }

  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'admin-background');
  }

  public goToDagboek(): void {
    this.router.navigate(['administration', 'dagboek']);
  }

  public goToApplyTemplate(): void {
    this.router.navigate(['administration', 'apply-template']);
  }

  goToTemplate() {
    this.router.navigate(['administration', 'template']);
  }
  goToAteliers() {
    this.router.navigate(['administration', 'ateliers']);
  }
  goToClients() {
    this.router.navigate(['administration', 'clients']);
  }
  goToBegeleiders() {
    this.router.navigate(['administration', 'begeleiders']);
  }
  goToEditBusRegeling() {
    this.router.navigate(['administration', 'busregeling']);
  }
  GoToPictoAgenda() {
    this.router.navigate(['administration', 'picto-view']);
  }

  goToWeekmenu() {
    this.router.navigate(['administration', 'menu', 'weekmenu']);
  }
  goToPictoViews() {
    this.router.navigate(['administration', 'picto-view']);
  }
}
