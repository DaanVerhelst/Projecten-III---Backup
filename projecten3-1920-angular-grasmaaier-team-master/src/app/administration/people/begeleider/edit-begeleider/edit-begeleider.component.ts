import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { Observable } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PeopleDataService } from '../../people-data.service';

@Component({
  selector: 'app-edit-begeleider',
  templateUrl: './edit-begeleider.component.html',
  styleUrls: ['./edit-begeleider.component.css']
})
export class EditBegeleiderComponent implements OnInit {

  @Input() begeleider$: IPersoon;
  @Input() id: number;

  @Output() begeleiders: EventEmitter<Observable<IPersoon[]>> = new EventEmitter();



  public actualBegeleider$: IPersoon;
  public begeleider: FormGroup;

  public isFileChosen: boolean;
  public fileName: string;
  public image: File;

  public showMsg: boolean;

  constructor(private fb: FormBuilder, private peopleDataService: PeopleDataService ) { }

  ngOnInit() {
    const reg = '[^.]+\.(jpg|jpeg|gif|tiff|bmp|png)';
    this.begeleider = this.fb.group({
      voornaam: ['', [Validators.required]],
      achternaam: ['', [Validators.required]],
      image: ['', Validators.pattern(reg)]
    });

  }

  onSubmit() {
  if (!this.begeleider.value.voornaam || this.begeleider.value.voornaam === undefined || this.begeleider.value.voornaam === '') {
    this.begeleider.value.voornaam = this.begeleider$.voornaam;
  }
  if (!this.begeleider.value.achternaam || this.begeleider.value.achternaam === undefined || this.begeleider.value.achternaam === '') {
    this.begeleider.value.achternaam = this.begeleider$.familienaam;
  }

  this.peopleDataService.putPersoonB$(this.begeleider$.id, {
      voornaam: this.begeleider.value.voornaam,
      familienaam: this.begeleider.value.achternaam,

    } as IPersoon)
      .subscribe();
  }

  isValid(field: string) {
    const input = this.begeleider.get(field);
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
