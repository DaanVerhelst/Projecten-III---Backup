import { Component, OnInit, Input } from '@angular/core';
import { IPersoon } from 'src/app/data-types/IPersoon';
import {
  faPlus,
  faCircle,
  faArrowRight,
  faArrowLeft
} from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { PictoViewDataService } from '../picto-view-data.service';
import { AngularWaitBarrier } from 'blocking-proxy/built/lib/angular_wait_barrier';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { IDag } from 'src/app/data-types/IDag';

@Component({
  selector: 'app-picto',
  templateUrl: './picto.component.html',
  styleUrls: ['./picto.component.css']
})
export class PictoComponent implements OnInit {
  protected selectedIndex = 0;

  protected faPlus = faPlus;
  protected faCircle = faCircle;
  protected faArrowRight = faArrowRight;
  protected faArrowLeft = faArrowLeft;
  protected numPages: number;

  @Input() client: IPersoon;
  ateliers$: Observable<IDag[]>;
  dagen: IDag[];
  clientId$: number;

  constructor(
    private fb: FormBuilder,
    private pictoDataService: PictoViewDataService
  ) {}

  ngOnInit() {
    this.clientId$ = this.client.id;

    this.ateliers$ = this.pictoDataService.getAteliersByDayByClient$(
      this.client.id
    );

    this.ateliers$.subscribe(a => {
      this.dagen = a;
    });
  }
}
