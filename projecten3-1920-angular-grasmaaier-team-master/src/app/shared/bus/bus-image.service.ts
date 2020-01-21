import { Injectable } from '@angular/core';
import { BusDataService } from './bus-data.service';
import { Observable, Subject } from 'rxjs';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';
@Injectable({
  providedIn: 'root'
})
export class BusImageService {
  public imageLoaded$ = new Subject<boolean>();
  public imageToShow$ = new Subject<any>();

  constructor(private dataService: BusDataService) { }

  public getImage(id: number): void {
    this.dataService.getBusImage$(id).subscribe(
      data => {
        if (this.dataService.imageCache.filter(i => i.id === id).length === 0) {
          const modal: IImageKeyStore = { image: data, id};
          this.dataService.imageCache.push(modal);
        }
        this.createImageFromBlob(data);
        this.imageLoaded$.next(true);
      }, error => {
        this.imageLoaded$.next(true);
      }
    );
  }

  public updateImage(id: number): void {
    this.dataService.updateBusImage(id);
  }

  public createImageFromFile(file: File) {
    const fileReader: FileReader = new FileReader();

    fileReader.onload = (e) => {
      this.imageToShow$.next(fileReader.result);
    };

    fileReader.readAsDataURL(file);
  }

  private createImageFromBlob(image: Blob): any {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
      this.imageToShow$.next(reader.result);
    }, false);
    if (image) {
       reader.readAsDataURL(image);
    }
  }
}
