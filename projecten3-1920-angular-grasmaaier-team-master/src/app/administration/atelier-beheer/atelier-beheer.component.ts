import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { AtelierDataService } from 'src/app/shared/atelier/atelier-data.service';
import { IAtelier } from 'src/app/data-types/IAtelier';

@Component({
  selector: 'app-atelier-beheer',
  templateUrl: './atelier-beheer.component.html',
  styleUrls: ['./atelier-beheer.component.css']
})
export class AtelierBeheerComponent implements OnInit {
  protected mode: Modes = Modes.Admin_Start;
  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Ateliers', value: ['ateliers']}
  ];
  protected atelierList: IAtelier[];
  protected errorMsg: string;
  public selectedAtelier: IAtelier;

  constructor(private atService: AtelierDataService) {}

  ngOnInit() {
    this.atelierList = [];
    this.selectedAtelier = null;
    this.atService.getAllAteliers$().subscribe((val) => {
      this.atelierList = val;
    }, (e) => {
      this.errorMsg = 'Kan momenteel de ateliers niet ophalen.';
    });
  }

  public changeError(error: string): void {
    this.errorMsg = error;
  }

  public selectionChanged(event: IAtelier): void {
    this.selectedAtelier = event;
  }

  public addNewAtelier(at: IAtelier): void {
    this.atelierList.push(at);
    this.atelierList = [...this.atelierList];
    this.selectedAtelier = at;
  }

  public deleteAtelier(at: IAtelier): void {
    const indx = this.atelierList.indexOf(at);
    this.atelierList.splice(indx, 1);
    this.atelierList = [...this.atelierList];
    this.selectedAtelier = null;
  }

  public changeAtelier(at: IAtelier): void {
    this.atService.updateAtelierImage(at.atelierID);
    const indx: number = this.atelierList.map(a => a.atelierID).indexOf(at.atelierID);
    this.atelierList[indx] = at;
    this.atelierList = [...this.atelierList];
    this.selectedAtelier = at;
  }
}
