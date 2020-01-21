import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PictoItemComponent } from './picto-item.component';

describe('PictoItemComponent', () => {
  let component: PictoItemComponent;
  let fixture: ComponentFixture<PictoItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PictoItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PictoItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
