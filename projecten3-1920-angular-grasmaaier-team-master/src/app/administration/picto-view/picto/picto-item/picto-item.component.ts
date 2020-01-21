import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { PictoViewDataService } from '../../picto-view-data.service';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
import { AtelierDataService } from 'src/app/shared/atelier/atelier-data.service';

@Component({
  selector: 'app-picto-item',
  templateUrl: './picto-item.component.html',
  styleUrls: ['./picto-item.component.css']
})
export class PictoItemComponent implements OnInit {


    @Input() public aterlier: IAtelier;
    public imageToShow: any;
    public loadedImg = false;

    constructor(private dataService: PictoViewDataService, private atelierpicodataservice: AtelierDataService) { }

    ngOnInit() {

      this.getImage();

    }


    private getImage(): void {
      this.atelierpicodataservice.getAtelierImage$(this.aterlier.atelierID).subscribe(
        data => {
          if (this.dataService.imageCache.filter(i => i.id === this.aterlier.atelierID).length === 0) {
            const modal: IImageKeyStore = { image: data, id: this.aterlier.atelierID};
            this.dataService.imageCache.push(modal);
          }
          this.loadedImg = true;
          this.createImageFromBlob(data);
        }, error => {
          this.loadedImg = true;
        }
      );
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
