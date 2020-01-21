import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { DagboekCategorie } from 'src/app/data-types/DagboekCategorie';
import { FormBuilder, Form, FormGroup } from '@angular/forms';
import { DagboekService } from '../dagboek.service';
import { Moment } from 'moment';

@Component({
  selector: 'app-notitie-veld',
  templateUrl: './notitie-veld.component.html',
  styleUrls: ['./notitie-veld.component.css']
})
export class NotitieVeldComponent implements OnInit, OnChanges {
  @Input() selectedCategorie: DagboekCategorie;
  @Input() comment: string;
  @Input() date: Moment;
  @Output() change = new EventEmitter<any>();

  public error: string;
  public form: FormGroup;

  constructor(private fb: FormBuilder, private service: DagboekService ) {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.form = this.fb.group({
      comment: [this.comment]
    });
  }

  protected submitForm() {
    if (this.enabled()) {
      this.comment = this.form.get('comment').value;
      this.change.next({
         cat: this.selectedCategorie,
         comment: this.comment
      });
      this.service.postNotities(this.date, this.selectedCategorie, this.comment).subscribe(() => {},
      (error: any) => {
        this.error = error;
      });
    }
  }

  protected enabled(): boolean {
    const touched: boolean = this.form.get('comment').touched;
    const value: string = this.form.get('comment').value;

    if (this.comment != null) {
      return touched && value !== this.comment;
    }

    return touched && value != null && value.length > 0;
  }
}
