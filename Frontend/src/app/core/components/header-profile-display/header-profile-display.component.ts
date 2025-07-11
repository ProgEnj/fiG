import { Component, OnInit } from '@angular/core';
import { LogoutComponent } from "../logout/logout.component";

@Component({
  selector: 'app-header-profile-display',
  imports: [LogoutComponent],
  templateUrl: './header-profile-display.component.html',
  styleUrl: './header-profile-display.component.scss'
})
export class HeaderProfileDisplayComponent implements OnInit {
  profileName: string = "Placeholder";

  ngOnInit(): void {
    let username = localStorage.getItem("username");
    if(username !== null) {
      this.profileName = username;
    }
  }
}