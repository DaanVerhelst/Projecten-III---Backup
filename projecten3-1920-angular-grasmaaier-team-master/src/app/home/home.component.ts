import { Component, OnInit } from '@angular/core';
import { Modes } from '../shared/navigation/navigations.modes';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  protected mode: Modes = Modes.Default;
  constructor() {

  }

  ngOnInit() {

  }

}
