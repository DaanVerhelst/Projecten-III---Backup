import { Component, OnInit, OnChanges, Input, SimpleChanges } from '@angular/core';
import { IBus } from 'src/app/data-types/IBus';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
import { BusDataService } from '../bus-data.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'bus-component',
  templateUrl: './bus.component.html',
  styleUrls: ['./bus.component.css']
})
export class BusComponent implements OnInit, OnChanges {
  @Input() public bus: any;
  public imageToShow: any;
  public loadedImg = false;

  constructor(private dataService: BusDataService) { }

  ngOnInit() {
    this.getImage();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getImage();
  }

  private getImage(): void {

    this.dataService.getBusImage$(this.bus.busID).subscribe(
      data => {
        if (this.dataService.imageCache.filter(i => i.id === this.bus.id).length === 0) {
          const modal: IImageKeyStore = { image: data, id: this.bus.id};
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
