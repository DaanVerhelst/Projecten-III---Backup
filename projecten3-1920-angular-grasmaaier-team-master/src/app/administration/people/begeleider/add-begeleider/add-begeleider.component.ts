import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { Observable } from 'rxjs';
import { PeopleDataService } from '../../people-data.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-begeleider',
  templateUrl: './add-begeleider.component.html',
  styleUrls: ['./add-begeleider.component.css']
})
export class AddBegeleiderComponent implements OnInit {

  public begeleider: FormGroup;
  public errorMsg: string;
  @Output() begeleiders: EventEmitter<Observable<IPersoon[]>> = new EventEmitter();

  public isFileChosen: boolean;
  public fileName: string;
  public image: File;

  public showMsg: boolean;

  constructor(private fb: FormBuilder, private peopleDataService: PeopleDataService) { }
// alles werkt behalven de fotoupload
  ngOnInit() {
    const reg = '[^.]+\.(jpg|jpeg|gif|tiff|bmp|png)';
    this.begeleider = this.fb.group({
      voornaam: ['', [Validators.required]],
      achternaam: ['', [Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      image: ['', [Validators.pattern(reg)]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      isAdmin: [''],
      isStagair: ['']
    });
  }

  onSubmit() {

    this.peopleDataService.postPersoonB$({
      voornaam: this.begeleider.value.voornaam,
      familienaam: this.begeleider.value.achternaam,
      password: this.begeleider.value.password,
      email: this.begeleider.value.email,
      selected: this.begeleider.value.isAdmin
    } as IPersoon)
      .subscribe(val => {
      this.showMsg = true;
      this.begeleider.reset();
      this.begeleiders.emit(this.peopleDataService.getBegeleiders$());
      if (this.isFileChosen) {
        this.uploadPicture(val);
      }
    });
    // TODO: CALL REGISTER METHOD WITH REQUIRED VARS
    this.peopleDataService.registerBegeleider$(this.begeleider.value.isAdmin, {
      email: this.begeleider.value.email,
      password: this.begeleider.value.password,
      voornaam: this. begeleider.value.voornaam,
      familienaam: this.begeleider.value.familienaam
    });
  }

  uploadPicture(id: any) {
    const uploadImage = new FormData();

    uploadImage.append('file', this.image);
    this.peopleDataService.postPictureB$(id, uploadImage)
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
      return 'is verplicht';
    } else if (errors.minlength) {
      return `needs at least ${
        errors.minlength.requiredLength
      } characters (got ${errors.minlength.actualLength})`;
    } else if (errors.pattern) {
      // return `Your image is not an image!`;
      return errors.errorMsg;
    }
  }

}
