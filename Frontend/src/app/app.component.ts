import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthenticationFormComponent } from "./authentication-form/authentication-form.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AuthenticationFormComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements AfterViewInit {

  @ViewChild(AuthenticationFormComponent) 
  auth!: AuthenticationFormComponent;

  ngAfterViewInit() {

  }

  title = 'Frontend';

}
