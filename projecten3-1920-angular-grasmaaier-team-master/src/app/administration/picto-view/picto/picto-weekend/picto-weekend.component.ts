import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PictoViewDataService } from '../../picto-view-data.service';

@Component({
  selector: 'app-picto-weekend',
  templateUrl: './picto-weekend.component.html',
  styleUrls: ['./picto-weekend.component.css']
})
export class PictoWeekendComponent implements OnInit {

  public weekend: FormGroup;
  @Input() clientID: number;
  @Input() date: Date;

  constructor(private fb: FormBuilder, private pictoDataService: PictoViewDataService) {

   }

  ngOnInit() {
    this.weekend = this.fb.group({
      zaterdag: ['', [Validators.required]],
      zondag: ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.pictoDataService.voegNotitieToeZaterdag( this.clientID, this.weekend.value.zaterdag).subscribe();
    this.pictoDataService.voegNotitieToeZondag(this.clientID, this.weekend.value.zondag).subscribe();
  }
}
