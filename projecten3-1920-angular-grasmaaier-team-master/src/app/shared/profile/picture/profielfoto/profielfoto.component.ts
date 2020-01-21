import { Component, OnInit, Input } from '@angular/core';
import { IPersoon } from 'src/app/data-types/IPersoon';
import { ProfielFotoDataService } from '../profielfoto-data.service';
import { PeopleDataService } from 'src/app/administration/people/people-data.service';

@Component({
  selector: 'app-profielfoto',
  templateUrl: './profielfoto.component.html',
  styleUrls: ['./profielfoto.component.css']
})
export class ProfielfotoComponent implements OnInit {

  @Input() public persoon: IPersoon;
  @Input() public Type: string;
  public imageToShow: any;
  public loadedImg = false;
  public tempPicture = 'https://www.pngkey.com/png/detail/157-1579943_no-profile-picture-round.png';

  constructor(private dataService: ProfielFotoDataService,
              private peopleDataService: PeopleDataService) { }

  ngOnInit() {


if (this.Type == '1') {
  this.dataService.getProfilePicBegeleider$(this.persoon.id).subscribe(
    data => {
      this.createImageFromBlob(data);
      this.loadedImg = false;
    }, error => {
      this.loadedImg = true;
    });
} else {
  this.dataService.getProfilePic$(this.persoon.id).subscribe(
    data => {
      this.createImageFromBlob(data);
      this.loadedImg = false;
    }, error => {
      this.loadedImg = true;
    }

  );
}

  }


  get image(): string {
    if (this.imageToShow == null ) {
      return null;
    } else {
      return this.imageToShow;
    }
  }

  private createImageFromBlob(image: Blob) {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
       this.imageToShow = reader.result;
    }, false);
    if (image) {
       reader.readAsDataURL(image);
    }
 }
}
