import { Component, inject } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.scss'
})
export class LogoutComponent {
  private _authService = inject(AuthenticationService);
  private _router = inject(Router);

  onClick() {
    this._authService.LogOut().subscribe(res => { 
      this._authService.ClearLoginInfo();
      this._router.navigate(['/refresh']).then(() => this._router.navigate(['/']));
    });
  }
}
