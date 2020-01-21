import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeComponent } from '../home/home.component';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';
import { SelectivePreloadStrategy } from './SelectivePreloadStrategy';
import { AuthGuard } from '../user/auth.guard';

const appRoutes: Routes = [
    // { path: 'home', component: HomeComponent },
    // { path: '', redirectTo: 'home', pathMatch: 'full'},
    { path: '', redirectTo: 'login', pathMatch: 'full'},
    {
      path: 'administration',
      canActivate: [AuthGuard],
      loadChildren: '../administration/administration.module#AdministrationModule',
      data: { preload: true }
    },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(appRoutes, {
      preloadingStrategy: SelectivePreloadStrategy
    })
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
