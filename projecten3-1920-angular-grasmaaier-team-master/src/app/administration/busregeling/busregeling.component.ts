import { Component, OnInit } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';

@Component({
  selector: 'app-busregeling',
  templateUrl: './busregeling.component.html',
  styleUrls: ['./busregeling.component.css']
})
export class BusregelingComponent implements OnInit {
  protected mode: Modes = Modes.Admin;
  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Busregeling', value: ['Busregeling']}
  ];
  constructor() { }

  ngOnInit() {
  }

}
