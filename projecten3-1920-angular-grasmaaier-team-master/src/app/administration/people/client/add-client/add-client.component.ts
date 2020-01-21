import { Component, OnInit, Output, EventEmitter  } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { PeopleDataService } from '../../people-data.service';
import { IPersoon } from 'src/app/data-types/IPersoon';

import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-client',
  templateUrl: './add-client.component.html',
  styleUrls: ['./add-client.component.css']
})
export class AddClientComponent implements OnInit {

  public client: FormGroup;
  public errorMsg: string;
  @Output() clienten: EventEmitter<Observable<IPersoon[]>> = new EventEmitter();

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
      image: ['', [Validators.pattern(reg)]]
    });
  }

  onSubmit() {
    this.peopleDataService.postPersoon$({
      voornaam: this.client.value.voornaam,
      familienaam: this.client.value.achternaam} as IPersoon)
      .subscribe(val => {
      this.showMsg = true;
      this.client.reset();
      // TODO: enkel doorgeven dat er aanpassing is. evt direct toevoegen aan observable
      // basis component lostrekken
      this.clienten.emit(this.peopleDataService.getClienten$());

      if (this.isFileChosen) {
        this.uploadPicture(val);
      }
    });
  }

  uploadPicture(id: any) {
    const uploadImage = new FormData();

    uploadImage.append('file', this.image);
    this.peopleDataService.postPicture$(id, uploadImage)
      .subscribe(val => {
        if (val) {

        } else {
          this.errorMsg = 'Profielfoto kon niet worden toegevoegd';
        }
      }, (err: HttpErrorResponse) => {

        if (err.error instanceof Error) {
          this.errorMsg = `Error tijdens het toevoegen van een foto: ${err.error.message}`;
        } else {
          this.errorMsg = `Error ${
            err.status
          } while trying to add Image: ${
            err.error
          }`;
        }
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
      return 'is verplicht';
    } else if (errors.minlength) {
      return `needs at least ${
        errors.minlength.requiredLength
      } characters (got ${errors.minlength.actualLength})`;
    } else if (errors.pattern) {
      return `Your image is not an image!`;
    }
  }

}
