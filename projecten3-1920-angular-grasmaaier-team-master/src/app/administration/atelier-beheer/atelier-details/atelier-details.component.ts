import { Component, OnInit, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { AtelierImageService } from 'src/app/shared/atelier/atelier-image.service';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { AtelierDataService } from 'src/app/shared/atelier/atelier-data.service';

@Component({
  selector: 'app-atelier-details',
  templateUrl: './atelier-details.component.html',
  styleUrls: ['./atelier-details.component.css']
})
export class AtelierDetailsComponent implements OnChanges, OnInit {
  @Input() atelier: IAtelier;

  @Output() errors = new EventEmitter<string>();
  @Output() veranderdAtelier = new EventEmitter<IAtelier>();
  @Output() nieuwAtelier = new EventEmitter<IAtelier>();
  @Output() verwijderdAtelier = new EventEmitter<IAtelier>();

  public namesAteliers: string[];
  protected imgData: any;
  public atelierFG: FormGroup;
  public isFileChosen: boolean;
  public fileName: string;
  public image: File;
  private reg = '[^.]+\.(jpg|jpeg|gif|tiff|bmp|png)';

  constructor(private fb: FormBuilder, private imageService: AtelierImageService,
              private atService: AtelierDataService) {}

  ngOnChanges() {
    this.changeVariables();
  }

  ngOnInit(): void {
    this.namesAteliers = null;
    this.resetFormGroup();
    this.imgData = null;
    this.atService.getNames$().subscribe(names => {
      this.namesAteliers = names;
    });
  }

  checkIfNameExists(nameControl: AbstractControl, names: string[]): {[key: string]: any} {
    if (names != null && names.map(n => n.toLowerCase()).includes(nameControl.value.toLowerCase())) {
      if (this.atelier != null && this.atelier.naam.toLocaleLowerCase() === nameControl.value.toLowerCase()) {
        return null;
      }

      return { nameNotUnique : true };
    }
    return null;
  }

  private changeVariables(): void {
    this.imageService.imageToShow$.subscribe(b => {
      this.imgData = b;
    });

    this.fileName = '';

    if (this.atelier != null) {
      this.imageService.getImage(this.atelier.atelierID);

      this.atelierFG =  this.atelierFG = this.fb.group({
        image: [''],
        name: ['',
          [Validators.required, Validators.minLength(3),
            (x: AbstractControl) => this.checkIfNameExists(x, this.namesAteliers)]]
      });

      this.isFileChosen = true;
      this.fileName = `${this.atelier.naam}.jpg`;
    } else {
      this.resetFormGroup();
      this.isFileChosen = false;
      this.imgData = null;
    }
  }

  resetFormGroup(): void {
    this.atelierFG = this.fb.group({
      image: ['', [Validators.pattern(this.reg), Validators.required]],
      name: ['',
        [Validators.required, Validators.minLength(3)
          , (x: AbstractControl) => this.checkIfNameExists(x, this.namesAteliers)]]
    });
  }

  getErrorMessage(errors: any) {
    if (!errors) {
      return null;
    }

    if (errors.required) {
      return 'is verplicht';
    } else if (errors.minlength) {
      return `moet minstens ${ errors.minlength.requiredLength} karakter lang zijn.`;
    } else if (errors.pattern) {
      return `is in een fout formaat`;
    } else if (errors.nameNotUnique) {
      return `wordt al gebruikt voor een ander atelier`;
    }
  }

  public isValid(field: string) {
    const input = this.atelierFG.get(field);
    return input.dirty && input.invalid;
  }

  public isTouched(field: string) {
    const input = this.atelierFG.get(field);
    return input.touched;
  }

  fieldClass(field: string) {
    return { 'is-invalid': this.isValid(field) };
  }

  public preUpload(event) {
    this.image = event.target.files[0];

    if (this.image === undefined && this.imgData != null) {
      this.fileName = null;
      this.isFileChosen = false;
      this.imgData = null;
      return;
    }

    if (event.target.files.length > 0) {
      this.isFileChosen = true;
    }

    this.fileName = this.image.name;

    if (this.fileName.length > 22) {
      this.fileName = this.fileName.substr(0, 22) + '...';
    }

    if (!this.isValid('image')) {
      this.imgData = this.imageService.createImageFromFile(this.image);
    }
  }

  public onSubmit(): void {
    const name: string = this.atelierFG.value.name;
    this.makeAtelier(name);
  }

  delete() {
    if (this.atelier != null && confirm(
        `Ben je zeker dat je het Atelier ${this.atelier.naam} wilt verwijderen?`
      )) {
        this.atService.removeAtelier$(this.atelier.atelierID).subscribe(() => {
          this.verwijderdAtelier.emit(this.atelier);
        }, (error) => {
          this.errors.emit('Er ging iets fout');
        });
    }
  }

  public makeAtelier(name: string) {
    if (this.atelier != null) {
      // Atelier bestond al
      this.atService.putAtelier$(this.atelier.atelierID, name).subscribe((val: number) => {
        if (this.image != null && this.isTouched('image')) {
          const uploadImage = new FormData();
          uploadImage.append('file', this.image);
          this.atService.updatePicture$(val, uploadImage).subscribe( () => {}, (error) => {
            this.errors.emit('Er ging iets fout');
          });
        }

        this.atService.getAtelier$(this.atelier.atelierID).subscribe((at) => {
          this.veranderdAtelier.emit(at);
        }, (error) => {
          this.errors.emit('Er ging iets fout');
        });
      }, (error: any) => {
        this.errors.emit('Er ging iets fout');
      });
    } else {
      this.atService.postAtelier$(name).subscribe((val: number) => {
        const uploadImage = new FormData();
        uploadImage.append('file', this.image);
        this.atService.postPicture$(val, uploadImage).subscribe(() => {
          // Get the new atelier
          this.atService.getAtelier$(val).subscribe((at) => {
            this.nieuwAtelier.emit(at);
          }, (error) => {
            this.errors.emit('Er ging iets fout');
          });
        }, (error) => {
          this.errors.emit('Er ging iets fout');
        });
      }, (error: any) => {
        this.errors.emit('Er ging iets fout');
      });
    }
  }
}
