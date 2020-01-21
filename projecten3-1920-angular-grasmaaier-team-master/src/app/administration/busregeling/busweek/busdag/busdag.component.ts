import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IBus } from 'src/app/data-types/IBus';

@Component({
  selector: 'app-busdag',
  templateUrl: './busdag.component.html',
  styleUrls: ['./busdag.component.css']
})
export class BusDagComponent implements OnInit {
  @Input() weekNr: number;
  @Input() dagNr: number;
  @Input() busList: IBus[];

  constructor(private router: Router) { }

  ngOnInit() {
  }

  clicked(event: any) {
    this.router.navigate(['administration', 'busregeling', 'week', this.weekNr , 'day', this.dagNr]);
  }
}
