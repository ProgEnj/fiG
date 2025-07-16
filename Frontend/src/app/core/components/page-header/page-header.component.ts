import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HeaderProfileDisplayComponent } from '../header-profile-display/header-profile-display.component';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-page-header',
  imports: [RouterLink, HeaderProfileDisplayComponent],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss'
})
export class PageHeaderComponent implements AfterViewInit {

  public _authService = inject(AuthenticationService);
  private _router = inject(Router);

  @ViewChild(HeaderProfileDisplayComponent) 
  profile!: HeaderProfileDisplayComponent;

  ngAfterViewInit() {
  }

  Navigate(path: string) {
    this._router.navigate([this._router.url, path]);
  }

}
