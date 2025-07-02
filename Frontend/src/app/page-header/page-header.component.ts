import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { AuthenticationFormComponent } from '../authentication-form/authentication-form.component';
import { LoginFormComponent } from '../login-form/login-form.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-page-header',
  imports: [ AuthenticationFormComponent, LoginFormComponent, RouterLink ],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss'
})
export class PageHeaderComponent implements AfterViewInit {

  @ViewChild(AuthenticationFormComponent) 
  auth!: AuthenticationFormComponent;

  @ViewChild(LoginFormComponent) 
  login!: LoginFormComponent;

  ngAfterViewInit() {

  }
}
