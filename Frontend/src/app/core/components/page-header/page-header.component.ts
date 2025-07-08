import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { AuthenticationFormComponent } from '../../../features/authentication-form/authentication-form.component';
import { LoginFormComponent } from '../../../features/login-form/login-form.component';
import { RouterLink } from '@angular/router';
import { HeaderProfileDisplayComponent } from '../header-profile-display/header-profile-display.component';

@Component({
  selector: 'app-page-header',
  imports: [ AuthenticationFormComponent, LoginFormComponent, RouterLink, HeaderProfileDisplayComponent ],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss'
})
export class PageHeaderComponent implements AfterViewInit {

  @ViewChild(HeaderProfileDisplayComponent) 
  profile!: HeaderProfileDisplayComponent;

  @ViewChild(AuthenticationFormComponent) 
  auth!: AuthenticationFormComponent;

  @ViewChild(LoginFormComponent) 
  login!: LoginFormComponent;

  isLoggedIn: boolean = true;

  ngAfterViewInit() {

  }

}
