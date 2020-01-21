import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PeopleDataService } from '../people-data.service';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { Observable } from 'rxjs';
import { IStringKeyStore } from 'src/app/data-types/IStringKeyStore';
import { AddClientComponent } from './add-client/add-client.component';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {

  @Input() id: number;

  client$: Observable<IPersoon[]>;
  public client: FormGroup;
  public showMsg: boolean;
  public tempPicture = 'https://www.pngkey.com/png/detail/157-1579943_no-profile-picture-round.png';
  components = [];

  public testID: number;
  public testPersoon: IPersoon;

  protected breadcrumb: IStringKeyStore[] = [
    {key: 'Menu', value: ['administration']},
    {key: 'Client', value: ['clients']}
  ];

  constructor(private peopleDataService: PeopleDataService) { }

  ngOnInit() {
    this.client$ = this.peopleDataService.getClienten$();
  }

  onSubmit() {
    this.peopleDataService.postPersoon$({} as IPersoon).subscribe(val => {
      this.showMsg = true;
      this.client.reset();
      this.client$ = this.peopleDataService.getClienten$();
    });
  }

  delete(id: number) {
    if (confirm('Are you sure you want to delete this client?')) {
      this.peopleDataService.deletePersoon$(id).subscribe(val => {
        this.client$ = this.peopleDataService.getClienten$();
      });
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

  addAnotherClient() {
    // THIS function needs to display a new add-client component nfi how
  }

  edit(pers: IPersoon, id: number) {
    // (id);
    this.testPersoon = pers;
    this.testID = id;
  }

  catchEventEmitter(event: Observable<IPersoon[]>) {

    this.client$ = event;
  }


}
