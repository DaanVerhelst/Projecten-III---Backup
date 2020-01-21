import { Component, OnInit, Renderer2, OnDestroy } from '@angular/core';
import { AbstractControl, FormGroup, FormBuilder, Validators, ValidatorFn } from '@angular/forms';
import { faUser, faKey } from '@fortawesome/free-solid-svg-icons';
import { Modes } from 'src/app/shared/navigation/navigations.modes';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AtelierDataService } from 'src/app/shared/atelier/atelier-data.service';
import { mergeMap, mergeAll, map } from 'rxjs/operators';
import { IImageKeyStore } from 'src/app/data-types/IImageKeyStore';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  public user: FormGroup;
  protected mode: Modes = Modes.Login;
  public errorMsg: string;

  // Font awesome
  public faUser = faUser;
  public faKey = faKey;

  constructor(
    private renderer: Renderer2,
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthenticationService,
    private atelierDataService: AtelierDataService
    ) { this.renderer.addClass(document.body, 'login-background'); }

  ngOnInit() {
    this.user = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'login-background');
  }

  onSubmit() {
    this.authService
    .login(this.user.value.username, this.user.value.password)
    .subscribe(
      val => {
        if (val) {
          if (this.authService.redirectUrl) {
            this.router.navigateByUrl(this.authService.redirectUrl);
            this.authService.redirectUrl = undefined;
          } else {
            this.loadAllAterlierImagesToCache();
            this.router.navigate(['administration']);
          }
        } else {
          this.errorMsg = `Kan niet inloggen.`;
        }
      },
      (err: HttpErrorResponse) => {
        if (err.error instanceof Error) {
          this.errorMsg = `Error bij het inloggen ${
            this.user.value.username
            }: ${err.error.message}.`;
        } else {
          if (err.status === 404) {
            this.errorMsg = 'Webserver lijkt offline, neem contact op met de IT-afdeling.';
          } else if (err.status === 400) {
            this.errorMsg = 'E-mailadres en of paswoord is verkeerd.';
          }
        }
      }
    );

  }

  loadAllAterlierImagesToCache() {
    this.atelierDataService.getAllAteliers$().subscribe(
      aterliers => aterliers.forEach(e => this.atelierDataService.getAtelierImage$(e.atelierID).subscribe(
        data => {
          if (this.atelierDataService.imageCache.filter(i => i.id === e.atelierID).length === 0) {
            const modal: IImageKeyStore = { image: data, id: e.atelierID};
            this.atelierDataService.imageCache.push(modal);
          }
        }
      ))
    );
  }
}
