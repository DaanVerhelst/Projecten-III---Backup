import { Component, OnInit, Input } from '@angular/core';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { Observable } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { PeopleDataService } from '../people-data.service';

@Component({
  selector: 'app-begeleider',
  templateUrl: './begeleider.component.html',
  styleUrls: ['./begeleider.component.css']
})
export class BegeleiderComponent implements OnInit {

  @Input() id: number;

  begeleider$: Observable<IPersoon[]>;
  public begeleider: FormGroup;
  public showMsg: boolean;

  public tempPicture = 'https://www.pngkey.com/png/detail/157-1579943_no-profile-picture-round.png';

  public testID: number;
  public testPersoon: IPersoon;

  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Begeleider', value: ['begeleiders']}
  ];

  constructor(private fb: FormBuilder, private peopleDataService: PeopleDataService) { }

  ngOnInit() {

    this.begeleider$ = this.peopleDataService.getBegeleiders$();
  }

  onSubmit() {
    this.peopleDataService.postPersoon$({} as IPersoon).subscribe(val => {
      this.showMsg = true;
      this.begeleider.reset();
      this.begeleider$ = this.peopleDataService.getBegeleiders$();
    });
  }

  delete(id: number) {
    if (confirm('Are you sure you want to delete this begeleider?')) {
      this.peopleDataService.deletePersoonBegeleider$(id).subscribe();
    }
  }

  getErrorMessage(errors: any) {
    if (!errors) {
      return null;
    }
    if (errors.required) {
      return 'is required';
    } else if (errors.minlength) {
      return `needs at least ${
        errors.minlength.requiredLength
      } characters (got ${errors.minlength.actualLength})`;
    } else if (errors.pattern) {
      return `Your image is not an image!`;
    }
  }

  edit(pers: IPersoon, id: number) {
    this.testPersoon = pers;
    this.testID = id;
  }

  catchEventEmitter(event: Observable<IPersoon[]>) {
    this.begeleider$ = event;
  }

}
