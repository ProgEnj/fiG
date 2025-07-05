import { Component } from '@angular/core';
import { PageHeaderComponent } from './core/components/page-header/page-header.component';
import { PageFooterComponent } from './core/components/page-footer/page-footer.component';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  imports: [ RouterOutlet, PageHeaderComponent, PageFooterComponent ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent {
  title = 'Frontend';
}
