import { Component, OnInit } from '@angular/core';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';

@Component({
  templateUrl: './template.component.html',
  styleUrls: ['./template.component.css']
})
export class TemplateComponent implements OnInit {
  protected mode: Modes = Modes.Admin;
  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Template', value: ['template']}
  ];
  constructor() { }

  ngOnInit() {
  }

}
