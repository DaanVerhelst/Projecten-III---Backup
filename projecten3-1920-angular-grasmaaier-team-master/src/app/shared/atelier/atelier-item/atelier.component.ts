import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { AtelierDataService } from '../atelier-data.service';
import { IAtelier } from 'src/app/data-types/IAtelier';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'atelier-component',
  templateUrl: './atelier.component.html',
  styleUrls: ['./atelier.component.css']
})
export class AtelierComponent implements OnInit, OnChanges {
  @Input() public aterlier: IAtelier;
  public imageToShow: any;
  public loadedImg = false;

  constructor(private dataService: AtelierDataService) { }

  ngOnInit() {
    this.getImage();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getImage();
  }

  private getImage(): void {
    this.dataService.getAtelierImage$(this.aterlier.atelierID).subscribe(
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
