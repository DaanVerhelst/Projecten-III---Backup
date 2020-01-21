import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { PeopleDataService } from '../../people-data.service';
import { Observable, Subject, from, of } from 'rxjs';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.css']
})
export class EditClientComponent implements OnInit {

  @Input() client$: IPersoon;
  @Input() id: number;

  // @Output() clienten$: Observable<IPersoon[]>;
  @Output() clienten: EventEmitter<Observable<IPersoon[]>> = new EventEmitter();

  public actualClient$: IPersoon;
  public client: FormGroup;

  public isFileChosen: boolean;
  public fileName: string;
  public image: File;

  public showMsg: boolean;

  constructor(private fb: FormBuilder, private peopleDataService: PeopleDataService) { }

  ngOnInit() {
    const reg = '[^.]+\.(jpg|jpeg|gif|tiff|bmp|png)';


    this.client = this.fb.group({
      voornaam: ['', [Validators.required]],
      achternaam: ['', [Validators.required]],
      image: ['', Validators.pattern(reg)]
    });
  }

  onSubmit() {
    this.peopleDataService.putPersoon$(this.id, {
      voornaam: this.client.value.voornaam,
      familienaam: this.client.value.achternaam} as IPersoon)
      .subscribe(val => {
      this.showMsg = true;
      this.clienten.emit(this.peopleDataService.getClienten$());

    });
  }

  isValid(field: string) {
    const input = this.client.get(field);
    return input.dirty && input.invalid;
  }

  fieldClass(field: string) {
    return { 'is-invalid': this.isValid(field) };
  }

  preUpload(event) {
    this.image = event.target.files[0];
    if (event.target.files.length > 0) {
      this.isFileChosen = true;
    }
    this.fileName = this.image.name;
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
}
