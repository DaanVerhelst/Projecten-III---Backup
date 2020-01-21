import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StartScreenComponent } from './start-screen/start-screen.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TimeFormatPipe } from './start-screen/pipes/time-format.pipe';
import { DayFormatPipe } from './start-screen/pipes/day-format.pipe';
import { DateFormatPipe } from './start-screen/pipes/date-format.pipe';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AtelierBeheerComponent } from './atelier-beheer/atelier-beheer.component';
import { AtelierComponent } from '../shared/atelier/atelier-item/atelier.component';
import { BusComponent } from '../shared/bus/bus-item/bus.component';
import { ClientComponent } from './people/client/client.component';
import { BegeleiderComponent } from './people/begeleider/begeleider.component';
import { AtelierDetailsComponent } from './atelier-beheer/atelier-details/atelier-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddClientComponent } from './people/client/add-client/add-client.component';
import { AtelierPipe } from '../shared/atelier/atelier.pipe';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditClientComponent } from './people/client/edit-client/edit-client.component';
import { AddBegeleiderComponent } from './people/begeleider/add-begeleider/add-begeleider.component';
import { EditBegeleiderComponent } from './people/begeleider/edit-begeleider/edit-begeleider.component';
import { BusregelingComponent } from './busregeling/busregeling.component';
import { BusDagComponent } from './busregeling/busweek/busdag/busdag.component';
import { BusWeekComponent } from './busregeling/busweek/busweek.component';
import { EditBusDayComponent } from './busregeling/edit-busday/edit-busday.component';
import { ApplyTemplateComponent } from './apply-template/apply-template/apply-template.component';
import {  MatDatepickerModule, MatNativeDateModule} from '@angular/material';
import { DatepickerComponent } from '../shared/datepicker/datepicker.component';
import { MomentDateModule } from '@angular/material-moment-adapter';
import { TemplateParentComponent } from './apply-template/template-parent/template-parent.component';
import { WeekmenuComponent } from './menu/weekmenu/weekmenu.component';
import { DagmenuComponent } from './menu/dagmenu/dagmenu.component';
import { DagboekComponent } from './dagboek/dagboek.component';
import { NotitieVeldComponent } from './dagboek/notitie-veld/notitie-veld.component';
import { PictoViewComponent } from './picto-view/picto-view.component';
import { PictoComponent } from './picto-view/picto/picto.component';
import { PictoDayComponent } from './picto-view/picto/picto-day/picto-day.component';
import { PictoWeekendComponent } from './picto-view/picto/picto-weekend/picto-weekend.component';
import { PictoItemComponent } from './picto-view/picto/picto-item/picto-item.component';


const routes = [
  { path: '', component: StartScreenComponent },
  { path: 'ateliers', component: AtelierBeheerComponent},
  { path: 'clients', component: ClientComponent},
  { path: 'begeleiders', component: BegeleiderComponent},
  { path: 'busregeling/week/:weekId/day/:dayId', component: EditBusDayComponent},
  { path: 'busregeling', component: BusregelingComponent},
  { path: 'apply-template', component: TemplateParentComponent},
  { path: 'apply-template/:date', component: TemplateParentComponent},
  { path: 'menu/weekmenu', component: WeekmenuComponent},
  { path: 'menu/dagmenu/:id', component: DagmenuComponent},
  { path: 'dagboek', component: DagboekComponent},
  { path: 'picto-view', component: PictoViewComponent}
  // { path: 'template/week/:weekId/day/:dayId/edit-people/:activityIndex', component: EditPeopleComponent}
];

@NgModule({
  declarations: [
    StartScreenComponent,
    TimeFormatPipe,
    DayFormatPipe,
    DateFormatPipe,
    BusregelingComponent,
    BusWeekComponent,
    BusDagComponent,
    EditBusDayComponent,
    ClientComponent,
    BegeleiderComponent,
    AtelierBeheerComponent,
    AtelierDetailsComponent,
    AddClientComponent,
    EditClientComponent,
    AddBegeleiderComponent,
    EditBegeleiderComponent,
    ApplyTemplateComponent,
    TemplateParentComponent,
    WeekmenuComponent,
    DagmenuComponent,
    DagboekComponent,
    NotitieVeldComponent,
    PictoViewComponent,
    PictoComponent,
    PictoItemComponent,
    PictoDayComponent,
    PictoWeekendComponent],

  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    DragDropModule,
    MatDatepickerModule,
    MomentDateModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AdministrationModule { }
