import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { RouteService } from './services/route.service';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    NavbarComponent,
    ToastModule,
    FormsModule,
  ],
  providers: [MessageService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Sky Sentinel';
  showNavbar = true;

  constructor(private routeService: RouteService) {
    this.routeService.currentUrl$.subscribe((url) => {
      this.setNavbarVisibility(url);
    });
  }

  setNavbarVisibility(url: string) {
    const NO_NAVBAR_PREFIXES = [
      '/login',
      '/register',
      '/confirm-email',
      '/forgot-password',
      '/check-email',
      '/reset-password',
    ];
    this.showNavbar = !NO_NAVBAR_PREFIXES.some((prefix) =>
      url.startsWith(prefix)
    );
  }
}
