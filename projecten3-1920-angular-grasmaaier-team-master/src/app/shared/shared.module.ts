import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationComponent } from './navigation/navigation.component';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AtelierComponent } from './atelier/atelier-item/atelier.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HourPickerComponent } from './hour-picker/hour-picker.component';
import { AtelierListComponent } from './atelier/atelier-list/atelier-list.component';
import { AtelierPipe } from './atelier/atelier.pipe';
import { ProfielfotoComponent } from './profile/picture/profielfoto/profielfoto.component';
import { BusComponent } from './bus/bus-item/bus.component';
import { WeekComponent } from './template/week/week.component';
import { DagComponent } from './template/week/dag/dag.component';
import { EditDayComponent } from './template/edit-day/edit-day.component';
import { EditPeopleComponent } from './template/edit-day/edit-people/edit-people.component';
import { TemplateComponent } from './template/template.component';
import { DagPipe } from './template/week/dag/dag.pipe';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { MatDatepickerModule } from '@angular/material';
import { MomentDateModule } from '@angular/material-moment-adapter';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes = [
  { path: 'template', component: TemplateComponent},
  { path: 'template/week/:weekId/day/:dayId', component: EditDayComponent},
  { path: 'concrete/day/:date', component: EditDayComponent}
];

@NgModule({
  declarations: [
    NavigationComponent,
    AtelierComponent,
    BusComponent,
    BreadcrumbComponent,
    HourPickerComponent,
    WeekComponent,
    DagComponent,
    DatepickerComponent,
    EditDayComponent,
    EditPeopleComponent,
    AtelierListComponent,
    AtelierPipe,
    TemplateComponent,
    DagPipe,
    ProfielfotoComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MomentDateModule,
    FontAwesomeModule,
    RouterModule,
    DragDropModule
  ],
  exports: [
    NavigationComponent,
    FontAwesomeModule,
    WeekComponent,
    DagComponent,
    EditPeopleComponent,
    DagPipe,
    DatepickerComponent,
    EditDayComponent,
    AtelierComponent,
    BusComponent,
    TemplateComponent,
    BreadcrumbComponent,
    HourPickerComponent,
    DragDropModule,
    AtelierListComponent,
    ProfielfotoComponent
  ]
})
export class SharedModule { }
